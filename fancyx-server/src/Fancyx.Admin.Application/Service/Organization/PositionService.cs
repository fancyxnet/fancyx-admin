using AutoMapper;
using Fancyx.Admin.Application.IService.Organization;
using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Admin.Application.Service.Organization.Models;
using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.EfCore;
using System.Data;

namespace Fancyx.Admin.Application.Service.Organization
{
    public class PositionService : IPositionService
    {
        private readonly IRepository<Position> _positionRepository;
        private readonly IRepository<PositionGroup> _positionGroupRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public PositionService(IRepository<Position> positionRepository, IRepository<PositionGroup> positionGroupRepository
            , IRepository<User> userRepository, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _positionGroupRepository = positionGroupRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        private async Task<List<PosistionLayerNames>> GetPosistionGroupNameAsync(List<long> ids)
        {
            var positions = await _positionRepository.Where(x => ids.Contains(x.Id)).SelectToListAsync(x => new { x.Id, x.GroupId });
            var groups = await _positionGroupRepository.GetListAsync(x => true);
            var list = new List<PosistionLayerNames>();

            foreach (var item in positions)
            {
                var single = new PosistionLayerNames
                {
                    Id = item.Id
                };
                var allGroups = groups.Where(x => x.Id == item.GroupId).Select(x => x.TreePath);
                foreach (var groupIds in allGroups)
                {
                    foreach (var groupId in groupIds.Split("/"))
                    {
                        single.LayerName += groups.Find(x => x.Id.ToString() == groupId)?.GroupName + "/";
                    }
                }
                single.LayerName = single.LayerName?.Trim('/');

                list.Add(single);
            }

            return list;
        }

        public async Task<bool> AddPositionAsync(AddOrUpdatePositionRequest req)
        {
            if (await _positionRepository.AnyAsync(x => x.Code.ToLower() == req.Code!.ToLower()))
            {
                throw new BusinessException("职位编号已存在");
            }
            var entity = _mapper.Map<AddOrUpdatePositionRequest, Position>(req);
            await _positionRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeletePositionAsync(long id)
        {
            var hasEmployees = await _userRepository.AnyAsync(x => x.PostId == id);
            if (hasEmployees) throw new BusinessException(message: "职位正在使用，不能删除");
            await _positionRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<PagedResult<PositionItem>> GetPositionListAsync(GetPositionListRequest req)
        {
            var pagedResp = await _positionRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Keyword), x => x.Name.Contains(req.Keyword!) || x.Code.Contains(req.Keyword!))
                .WhereIf(req.Level > 0, x => x.Level == req.Level)
                .WhereIf(req.Status > 0, x => x.Status == req.Status)
                .WhereIf(req.GroupId.HasValue, x => x.GroupId == req.GroupId)
                .OrderBy(x => x.Level)
                .ThenByDescending(x => x.CreationTime)
                .PagedAsync(req.Current, req.PageSize);
            var ids = pagedResp.Items.Select(x => x.Id).ToList();
            var list = _mapper.Map<List<Position>, List<PositionItem>>(pagedResp.Items);
            var names = await GetPosistionGroupNameAsync(ids);
            foreach (var item in list)
            {
                var tmp = names.FirstOrDefault(x => x.Id == item.Id);
                item.LayerName = tmp?.LayerName;
            }
            return new PagedResult<PositionItem>(pagedResp.Total, list);
        }

        public async Task<bool> UpdatePositionAsync(AddOrUpdatePositionRequest req)
        {
            if (!req.Id.HasValue) throw new ArgumentNullException(nameof(req.Id));
            var entity = await _positionRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            string code = req.Code!.ToLower();
            if (entity.Code.ToLower() != code && await _positionRepository.AnyAsync(x => x.Code.ToLower() == code))
            {
                throw new BusinessException("职位编号已存在");
            }

            entity.Name = req.Name;
            entity.Code = req.Code;
            entity.Level = req.Level;
            entity.Status = req.Status;
            entity.Description = req.Description;
            entity.GroupId = req.GroupId;
            await _positionRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<List<AppOptionTree>> GetPositionTreeOptionAsync()
        {
            var groups = await _positionGroupRepository.GetListAsync(x => true);
            var positions = await _positionRepository.GetListAsync(x => true);
            var topGroups = groups.Where(x => !x.ParentId.HasValue).ToList();
            var list = new List<AppOptionTree>();
            List<AppOptionTree> GetChildren(string id)
            {
                var items = groups.Where(x => x.ParentId.ToString() == id);
                var children = new List<AppOptionTree>();
                if (items.Any())
                {
                    foreach (var item in items)
                    {
                        var t = new AppOptionTree()
                        {
                            Label = item.GroupName,
                            Value = item.Id.ToString()
                        };
                        t.Children = GetChildren(t.Value);
                        children.Add(t);
                        //最底级查职位
                        if (t.Children.Count == 0)
                        {
                            t.Children = positions.Where(x => x.GroupId.ToString() == t.Value).Select(x => new AppOptionTree
                            {
                                Label = x.Name,
                                Value = x.Id.ToString()
                            }).ToList();
                        }
                    }
                }
                else
                {
                    children = positions.Where(x => x.GroupId.ToString() == id).Select(x => new AppOptionTree
                    {
                        Label = x.Name,
                        Value = x.Id.ToString()
                    }).ToList();
                }
                return children;
            }

            foreach (var group in topGroups)
            {
                var t = new AppOptionTree()
                {
                    Label = group.GroupName,
                    Value = group.Id.ToString()
                };
                t.Children = GetChildren(t.Value);
                list.Add(t);
            }
            return list;
        }

        public async Task<PositionItem> GetPositionAsync(long id)
        {
            var data = await _positionRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return _mapper.Map<PositionItem>(data);
        }
    }
}