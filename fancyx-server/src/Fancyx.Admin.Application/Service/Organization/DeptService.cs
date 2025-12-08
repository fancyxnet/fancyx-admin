using AutoMapper;
using Fancyx.Admin.Application.IService.Organization;
using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Core.Interfaces;
using Fancyx.EfCore;
using Fancyx.Shared.EfCore;

using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Organization
{
    public class DeptService : IDeptService
    {
        private readonly IRepository<Dept> _deptRepository;
        private readonly IRepository<User> _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;

        public DeptService(IRepository<Dept> deptRepository, IRepository<User> userRepository, ICurrentUser currentUser, IMapper mapper)
        {
            _deptRepository = deptRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<bool> AddDeptAsync(AddOrUpdateDeptRequest req)
        {
            if (await _deptRepository.Where(x => x.Code.ToLower() == req.Code!.ToLower()).AnyAsync())
            {
                throw new BusinessException(message: "部门编号已存在");
            }

            var entity = _mapper.Map<AddOrUpdateDeptRequest, Dept>(req);
            entity.ParentId = req.ParentId;
            entity.Code = req.Code;
            entity.SetTreeProperties(await _deptRepository.FindAsync(req.ParentId));

            await _deptRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeleteDeptAsync(long id)
        {
            var hasChildren = await _deptRepository.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
            {
                throw new BusinessException("存在子部门，无法删除");
            }

            var hasEmployees = await _userRepository.AnyAsync(x => x.DeptId == id);
            if (hasEmployees) throw new BusinessException(message: "部门下存在用户，不能删除");
            await _deptRepository.DeleteAsync(x => id == x.Id);
            return true;
        }

        public async Task<List<DeptItem>> GetDeptListAsync(GetDeptListRequest req)
        {
            bool hasFilter = !string.IsNullOrEmpty(req.Name) || !string.IsNullOrEmpty(req.Code) || req.Status > 0;
            if (hasFilter)
            {
                var filter = await _deptRepository.GetQueryable().PowerFilter(_currentUser)
                    .WhereIf(!string.IsNullOrEmpty(req.Name), x => x.Name.Contains(req.Name!))
                    .WhereIf(!string.IsNullOrEmpty(req.Code), x => x.Code.Contains(req.Code!)) // ==
                    .WhereIf(req.Status > 0, x => x.Status == req.Status) // ==
                    .OrderBy(x => x.Sort).ToListAsync();
                var result = _mapper.Map<List<Dept>, List<DeptItem>>(filter);

                return result;
            }

            var allNodes = await _deptRepository.GetQueryable().PowerFilter(_currentUser).OrderBy(x => x.TreePath).ToDictionaryAsync(k => k.Id);
            var tree = new List<DeptItem>();
            var nodeDtos = new Dictionary<long, DeptItem>();
            var endDtos = new List<DeptItem>();

            foreach (var node in allNodes.Values)
            {
                var tmp = _mapper.Map<Dept, DeptItem>(node);
                nodeDtos[tmp.Id] = tmp;
                if (node.ParentId.HasValue)
                {
                    if (nodeDtos.TryGetValue(node.ParentId.Value, out var parent))
                    {
                        parent.Children ??= [];
                        parent.Children.Add(tmp);
                        parent.Children = parent.Children.OrderBy(s => s.Sort).ToList();
                    }
                    else
                    {
                        endDtos.Add(tmp);
                    }
                }
                else
                {
                    tree.Add(tmp);
                }
            }
            return tree.OrderBy(x => x.Sort).Concat(endDtos).ToList();
        }

        public async Task<bool> UpdateDeptAsync(AddOrUpdateDeptRequest req)
        {
            if (!req.Id.HasValue) throw new ArgumentNullException(nameof(req.Id));

            var entity = await _deptRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            if (!entity.Code.Equals(req.Code, StringComparison.CurrentCultureIgnoreCase) &&
                await _deptRepository.AnyAsync(x => x.Code.ToLower() == req.Code!.ToLower()))
            {
                throw new BusinessException(message: "部门编号已存在");
            }

            if (req.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为上级部门");
            }

            entity.Name = req.Name;
            entity.Code = req.Code;
            entity.Sort = req.Sort;
            entity.Description = req.Description;
            entity.Status = req.Status;
            entity.CuratorId = req.CuratorId;
            entity.Email = req.Email;
            entity.Phone = req.Phone;
            entity.ParentId = req.ParentId;
            if (entity.ParentId.HasValue)
            {
                var parentIsSub = await _deptRepository.AnyAsync(x => x.Id == entity.ParentId && x.ParentId == entity.Id);
                if (parentIsSub)
                {
                    throw new BusinessException("不能选择子部门作为上级部门");
                }
                entity.SetTreeProperties(await _deptRepository.FindAsync(entity.ParentId));
            }

            await _deptRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<List<DeptSimpleInfo>> GetDeptSimpleInfosAsync(string? keyword)
        {
            var depts = await _deptRepository.GetQueryable().PowerFilter(_currentUser).WhereIf(!string.IsNullOrEmpty(keyword),
                    x => x.Name.StartsWith(keyword!) || x.Code.StartsWith(keyword!))
                .SelectToListAsync(x => new { x.Id, x.Name, x.Code, x.ParentId, x.Sort, x.CreationTime, x.TreeLevel });
            var list = new List<DeptSimpleInfo>();
            //顶级部门放前面
            var topDepts = depts.Where(x => !x.ParentId.HasValue).OrderBy(x => x.TreeLevel)
                .ThenBy(x => x.Sort).ThenBy(x => x.CreationTime).ToList();
            topDepts.ForEach(x => { list.Add(new DeptSimpleInfo { Id = x.Id, Name = x.Name, Code = x.Code }); });
            //子部门放后面
            depts.Where(x => x.ParentId.HasValue).OrderBy(x => x.TreeLevel).ThenBy(x => x.Sort).ThenBy(x => x.CreationTime).ToList().ForEach(x =>
            {
                list.Add(new DeptSimpleInfo { Id = x.Id, Name = x.Name, Code = x.Code });
            });
            return list;
        }
    }
}