using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Admin.SharedService;
using Fancyx.EfCore;
using Fancyx.EfCore.Aop;
using Fancyx.Shared.EfCore;

namespace Fancyx.Admin.Service.System
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RoleMenu> _roleMenuRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IdentitySharedService _identitySharedService;
        private readonly IRepository<RoleDept> _roleDeptRepository;
        private readonly IRepository<Dept> _deptRepository;

        public RoleService(IRepository<Role> roleRepository, IRepository<RoleMenu> roleMenuRepository
            , IRepository<UserRole> userRoleRepository, IdentitySharedService identitySharedService
            , IRepository<RoleDept> roleDeptRepository, IRepository<Dept> deptRepository)
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

            var entity = new Role
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
                var items = new List<RoleMenu>();
                foreach (var item in dto.MenuIds)
                {
                    items.Add(new RoleMenu
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

            var role = await _roleRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            if (role.RoleName == DataPower.SuperAdmin)
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
            ArgumentNullException.ThrowIfNull(dto.Id);
            var entity = await _roleRepository.FindAsync(dto.Id) ?? throw new BusinessException("数据不存在");
            var isExist = await _roleRepository.AnyAsync(x => x.RoleName.ToLower() == dto.RoleName.ToLower());
            if (entity.RoleName.ToLower() != dto.RoleName.ToLower() && isExist)
            {
                throw new BusinessException("角色名已存在");
            }

            if (entity.RoleName == DataPower.SuperAdmin)
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
            var role = await _roleRepository.FindAsync(roleId);
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
            var role = await _roleRepository.FindAsync(dto.RoleId);
            if (role == null) throw new BusinessException("角色不存在");

            await _roleDeptRepository.DeleteAsync(x => x.RoleId == dto.RoleId);
            if (dto is { DeptPowerType: DeptPowerType.Specify, DeptIds.Length: > 0 })
            {
                var roleDeptList = dto.DeptIds
                    .Select(deptId => new RoleDept { DeptId = deptId, RoleId = dto.RoleId }).ToList();
                await _roleDeptRepository.InsertManyAsync(roleDeptList);
            }

            role.DeptPowerType = dto.DeptPowerType;
            await _roleRepository.UpdateAsync(role);
        }
    }
}