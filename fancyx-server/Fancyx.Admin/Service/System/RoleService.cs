using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.Entities.System;
using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.Repository;
using Fancyx.Repository.Aop;
using Fancyx.Shared.Consts;
using Fancyx.Shared.Enums;

namespace Fancyx.Admin.Service.System
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<RoleDO> _roleRepository;
        private readonly IRepository<RoleMenuDO> _roleMenuRepository;
        private readonly IRepository<UserRoleDO> _userRoleRepository;
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<RoleDeptDO> _roleDeptRepository;
        private readonly IRepository<DeptDO> _deptRepository;

        public RoleService(IRepository<RoleDO> roleRepository, IRepository<RoleMenuDO> roleMenuRepository
            , IRepository<UserRoleDO> userRoleRepository, IdentitySharedService identitySharedService
            , IRepository<RoleDeptDO> roleDeptRepository, IRepository<DeptDO> deptRepository)
        {
            _roleRepository = roleRepository;
            _roleMenuRepository = roleMenuRepository;
            _userRoleRepository = userRoleRepository;
            _identitySharedService = identitySharedService;
            _roleDeptRepository = roleDeptRepository;
            _deptRepository = deptRepository;
        }

        public async Task<bool> AddRoleAsync(RoleDto dto)
        {
            var isExist = await _roleRepository.AnyAsync(x => x.RoleName.ToLower() == dto.RoleName.ToLower());
            if (isExist)
            {
                throw new BusinessException("角色名已存在");
            }

            var entity = new RoleDO
            {
                RoleName = dto.RoleName,
                Remark = dto.Remark
            };
            await _roleRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> AssignMenuAsync(AssignMenuDto dto)
        {
            await _roleMenuRepository.DeleteAsync(x => x.RoleId == dto.RoleId);
            if (dto.MenuIds != null)
            {
                var items = new List<RoleMenuDO>();
                foreach (var item in dto.MenuIds)
                {
                    items.Add(new RoleMenuDO
                    {
                        RoleId = dto.RoleId,
                        MenuId = item
                    });
                }

                if (items.Count > 0)
                {
                    await _roleMenuRepository.InsertManyAsync(items);
                }
            }

            await _identitySharedService.DelUserPermissionCacheByRoleIdAsync(dto.RoleId);
            return true;
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var hasUsers = await _userRoleRepository.AnyAsync(x => x.RoleId == id);
            if (hasUsers) throw new BusinessException(message: "角色已分配给用户，不能删除");

            var role = await _roleRepository.GetAsync(x => x.Id == id) ?? throw new EntityNotFoundException();
            if (role.RoleName == AdminConsts.SuperAdminRole)
            {
                throw new BusinessException(message: $"{role.RoleName}不能删除");
            }

            await _roleRepository.DeleteAsync(x => x.Id == id);
            await _identitySharedService.DelUserPermissionCacheByRoleIdAsync(id);
            return true;
        }

        public async Task<PagedResult<RoleListDto>> GetRoleListAsync(RoleQueryDto dto)
        {
            var resp = await _roleRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.RoleName), x => x.RoleName.Contains(dto.RoleName!))
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new RoleListDto() { Id = x.Id, IsEnabled = x.IsEnabled, Remark = x.Remark, CreationTime = x.CreationTime, RoleName = x.RoleName })
                .PagedAsync(dto.Current, dto.PageSize);

            return new PagedResult<RoleListDto>(resp.Total, resp.Items);
        }

        public async Task<List<AppOption>> GetRoleOptionsAsync()
        {
            return await _roleRepository.GetQueryable().SelectToListAsync(x => new AppOption
            {
                Label = x.RoleName,
                Value = x.Id.ToString()
            });
        }

        public async Task<bool> UpdateRoleAsync(RoleDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _roleRepository.GetAsync(x => x.Id == dto.Id) ?? throw new BusinessException("数据不存在");
            var isExist = await _roleRepository.AnyAsync(x => x.RoleName.ToLower() == dto.RoleName.ToLower());
            if (entity.RoleName.ToLower() != dto.RoleName.ToLower() && isExist)
            {
                throw new BusinessException("角色名已存在");
            }

            if (entity.RoleName == AdminConsts.SuperAdminRole)
            {
                throw new BusinessException(message: $"{entity.RoleName}不允许编辑");
            }

            entity.RoleName = dto.RoleName;
            entity.Remark = dto.Remark;
            entity.IsEnabled = dto.IsEnabled;
            await _roleRepository.UpdateAsync(entity);

            if (!entity.IsEnabled)
            {
                await _identitySharedService.DelUserPermissionCacheByRoleIdAsync(entity.Id);
            }

            return true;
        }

        public async Task<Guid[]> GetRoleMenuIdsAsync(Guid id)
        {
            return [.. await _roleMenuRepository.Where(x => x.RoleId == id).SelectToListAsync(x => x.MenuId)];
        }

        public async Task<(RolePowerInfoDto, List<DeptTreeOptionDto>)> GetRoleDeptPowerInfoAsync(Guid roleId)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == roleId);
            if (role == null) return (new RolePowerInfoDto(), []);

            var info = new RolePowerInfoDto()
            {
                DeptPowerType = role.DeptPowerType
            };
            if (info.DeptPowerType == DeptPowerType.Specify)
            {
                info.DeptIds = await _roleDeptRepository.Where(x => x.RoleId == roleId).SelectToListAsync(x => x.DeptId);
            }

            var allDept = await _deptRepository.GetQueryable().SelectToListAsync(x => new { x.Id, x.Name, x.Code, x.ParentId });
            info.AllDeptIds = allDept.Select(x => x.Id).ToList();
            var rootDept = allDept.Where(x => !x.ParentId.HasValue).ToList();
            var resultList = new List<DeptTreeOptionDto>();
            rootDept.ForEach(x =>
            {
                var tmp = new DeptTreeOptionDto
                {
                    Title = x.Name,
                    Key = x.Id,
                    Children = GetChildren(x.Id)
                };
                resultList.Add(tmp);
            });

            return (info, resultList);

            List<DeptTreeOptionDto>? GetChildren(Guid itemId)
            {
                var subDeptList = allDept.Where(x => x.ParentId == itemId).ToList();
                if (subDeptList.Count == 0) return null;
                var children = new List<DeptTreeOptionDto>();
                subDeptList.ForEach(x =>
                {
                    var tmp = new DeptTreeOptionDto
                    {
                        Title = x.Name,
                        Key = x.Id,
                        Children = GetChildren(x.Id)
                    };
                    children.Add(tmp);
                });

                return children;
            }
        }

        [AsyncTransactional]
        public async Task AssignDataScopeAsync(AssignDataScopeDto dto)
        {
            var role = await _roleRepository.GetAsync(x => x.Id == dto.RoleId);
            if (role == null) throw new BusinessException("角色不存在");

            await _roleDeptRepository.DeleteAsync(x => x.RoleId == dto.RoleId);
            if (dto is { DeptPowerType: DeptPowerType.Specify, DeptIds.Length: > 0 })
            {
                var roleDeptList = dto.DeptIds
                    .Select(deptId => new RoleDeptDO { DeptId = deptId, RoleId = dto.RoleId }).ToList();
                await _roleDeptRepository.InsertManyAsync(roleDeptList);
            }

            role.DeptPowerType = dto.DeptPowerType;
            await _roleRepository.UpdateAsync(role);
        }
    }
}