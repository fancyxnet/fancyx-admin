using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Helpers;
using Fancyx.Repository;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.Organization
{
    public class DeptService : IDeptService
    {
        private readonly IRepository<DeptDO> _deptRepository;
        private readonly IRepository<EmployeeDO> _employeeRepository;

        public DeptService(IRepository<DeptDO> deptRepository, IRepository<EmployeeDO> employeeRepository)
        {
            _deptRepository = deptRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> AddDeptAsync(DeptDto dto)
        {
            if (await _deptRepository.Where(x => x.Code.ToLower() == dto.Code!.ToLower()).AnyAsync())
            {
                throw new BusinessException(message: "部门编号已存在");
            }

            var entity = AutoMapperHelper.Instance.Map<DeptDto, DeptDO>(dto);
            entity.ParentId = dto.ParentId;
            entity.Code = dto.Code;
            if (entity.ParentId.HasValue)
            {
                var all = await _deptRepository.GetListAsync(x => true);
                int layer = 1;
                entity.ParentIds = GetParentIds(all, entity.ParentId.Value, ref layer);
                entity.Layer = layer;
            }

            await _deptRepository.InsertAsync(entity);
            return true;
        }

        public string GetParentIds(List<DeptDO> all, Guid id, ref int layer)
        {
            layer += 1;
            var parentId = all.Find(x => x.Id == id)?.ParentId;
            if (parentId == null) return id.ToString();
            return GetParentIds(all, parentId.Value, ref layer) + "," + id;
        }

        public async Task<bool> DeleteDeptAsync(Guid id)
        {
            var hasChildren = await _deptRepository.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
            {
                throw new BusinessException("存在子部门，无法删除");
            }

            var hasEmployees = await _employeeRepository.AnyAsync(x => x.DeptId == id);
            if (hasEmployees) throw new BusinessException(message: "部门下存在员工，不能删除");
            await _deptRepository.DeleteAsync(x => id == x.Id);
            return true;
        }

        public async Task<List<DeptListDto>> GetDeptListAsync(DeptQueryDto dto)
        {
            bool hasFilter = !string.IsNullOrEmpty(dto.Name) || !string.IsNullOrEmpty(dto.Code)
                                                             || dto.Status > 0;
            if (hasFilter)
            {
                var filter = await _deptRepository.GetQueryable()
                    .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.Contains(dto.Name!))
                    .WhereIf(!string.IsNullOrEmpty(dto.Code), x => x.Code.Contains(dto.Code!)) // ==
                    .WhereIf(dto.Status > 0, x => x.Status == dto.Status) // ==
                    .OrderBy(x => x.Sort).ToListAsync();
                var result = AutoMapperHelper.Instance.Map<List<DeptDO>, List<DeptListDto>>(filter);

                // Add curator names for filtered results
                await AddCuratorNames(result); // ++

                return result;
            }

            var all = await _deptRepository.GetQueryable().OrderBy(x => x.ParentIds).ToListAsync();
            var tree = AutoMapperHelper.Instance.Map<List<DeptDO>, List<DeptListDto>>(all.Where(x => x.ParentId == null)
                .OrderBy(t => t.Sort).ToList());

            // Add curator names for all departments
            await AddCuratorNames(tree); // ++

            foreach (var item in tree)
            {
                item.Children = getChildren(item.Id)?.OrderBy(x => x.Sort).ToList();
            }

            List<DeptListDto>? getChildren(Guid id)
            {
                var children =
                    AutoMapperHelper.Instance.Map<List<DeptDO>, List<DeptListDto>>(all.Where(x => x.ParentId == id)
                        .ToList());
                if (children.Count <= 0) return null;

                // Add curator names for child departments
                AddCuratorNames(children).Wait(); // ++

                foreach (var item in children)
                {
                    item.Children = getChildren(item.Id);
                }

                return children;
            }

            return tree;
        }

        private async Task AddCuratorNames(List<DeptListDto> depts)
        {
            var curatorIds = depts.Select(d => d.CuratorId).Where(id => id.HasValue).Distinct().ToList();

            if (curatorIds.Any())
            {
                var employees = await _employeeRepository
                    .Where(e => curatorIds.Contains(e.Id))
                    .SelectToListAsync(e => new { e.Id, e.Name });

                var employeeDict = employees.ToDictionary(e => e.Id, e => e.Name);

                foreach (var dept in depts)
                {
                    if (dept.CuratorId.HasValue && employeeDict.TryGetValue(dept.CuratorId.Value, out var name))
                    {
                        dept.CuratorName = name;
                    }
                }
            }
        }

        public async Task<bool> UpdateDeptAsync(DeptDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));

            var entity = await _deptRepository.Where(x => x.Id == dto.Id).FirstAsync();
            if (!entity.Code.Equals(dto.Code, StringComparison.CurrentCultureIgnoreCase) &&
                await _deptRepository.AnyAsync(x => x.Code.ToLower() == dto.Code!.ToLower()))
            {
                throw new BusinessException(message: "部门编号已存在");
            }

            if (dto.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为上级部门");
            }

            entity.Name = dto.Name;
            entity.Code = dto.Code;
            entity.Sort = dto.Sort;
            entity.Description = dto.Description;
            entity.Status = dto.Status;
            entity.CuratorId = dto.CuratorId;
            entity.Email = dto.Email;
            entity.Phone = dto.Phone;
            entity.ParentId = dto.ParentId;
            if (entity.ParentId.HasValue)
            {
                var parentIsSub = await _deptRepository
                    .Where(x => x.Id == entity.ParentId.Value && x.ParentId == entity.Id).AnyAsync();
                if (parentIsSub)
                {
                    throw new BusinessException("不能选择子部门作为上级部门");
                }

                var all = await _deptRepository.GetListAsync(x => true);
                int layer = 1;
                entity.ParentIds = GetParentIds(all, entity.ParentId.Value, ref layer);
                entity.Layer = layer;
            }

            await _deptRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<List<DeptSimpleInfoDto>> GetDeptSimpleInfosAsync(string? keyword)
        {
            var depts = await _deptRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(keyword),
                    x => x.Name.StartsWith(keyword!) || x.Code.StartsWith(keyword!))
                .SelectToListAsync(x => new { x.Id, x.Name, x.Code, x.ParentId, x.Sort });
            var list = new List<DeptSimpleInfoDto>();
            //顶级部门放前面
            var topDepts = depts.Where(x => !x.ParentId.HasValue || x.ParentId == Guid.Empty).OrderBy(x => x.Sort)
                .ThenBy(x => x.Name).ToList();
            topDepts.ForEach(x => { list.Add(new DeptSimpleInfoDto { Id = x.Id, Name = x.Name, Code = x.Code }); });
            //子部门放后面
            depts.Where(x => x.ParentId.HasValue).OrderBy(x => x.Sort).ThenBy(x => x.Name).ToList().ForEach(x =>
            {
                list.Add(new DeptSimpleInfoDto { Id = x.Id, Name = x.Name, Code = x.Code });
            });
            return list;
        }
    }
}