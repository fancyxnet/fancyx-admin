using AutoMapper;
using Fancyx.Admin.Application.IService.Organization;
using Fancyx.Admin.Application.IService.Organization.Models;
using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.EfCore;
using Fancyx.Utils;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Application.Service.Organization
{
    public class PositionGroupService : IPositionGroupService
    {
        private readonly IRepository<PositionGroup> _positionGroupRepository;
        private readonly IRepository<Position> _positionRepository;
        private readonly IMapper _mapper;

        public PositionGroupService(IRepository<PositionGroup> positionGroupRepository,
            IRepository<Position> positionRepository, IMapper mapper)
        {
            _positionGroupRepository = positionGroupRepository;
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddPositionGroupAsync(AddOrUpdatePositionGroupRequest req)
        {
            var entity = _mapper.Map<AddOrUpdatePositionGroupRequest, PositionGroup>(req);
            entity.SetTreeProperties(await _positionGroupRepository.FindAsync(req.ParentId));

            await _positionGroupRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeletePositionGroupAsync(long id)
        {
            var hasChildren = await _positionGroupRepository.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
            {
                throw new BusinessException("存在子分组，不能删除");
            }

            var hasPositions = await _positionRepository.AnyAsync(x => x.GroupId == id);
            if (hasPositions)
            {
                throw new BusinessException(message: "分组下有职位，不能删除");
            }

            await _positionGroupRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<List<PositionGroupItem>> GetPositionGroupListAsync(GetPositionGroupListRequest req)
        {
            if (!string.IsNullOrEmpty(req.GroupName))
            {
                return await _positionGroupRepository.GetQueryable()
                .Where(x => x.GroupName.Contains(req.GroupName!))
                .Select(x => new PositionGroupItem
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    Remark = x.Remark,
                    ParentId = x.ParentId,
                    Sort = x.Sort,
                }).OrderBy(x => x.Sort).ToListAsync();
            }

            var all = await _positionGroupRepository.GetQueryable().ToListAsync();

            return CollectionUtils.BuildTree(all, g => new PositionGroupItem
            {
                Id = g.Id,
                GroupName = g.GroupName,
                Remark = g.Remark,
                ParentId = g.ParentId,
                Sort = g.Sort,
            }, g => g.Id, g => g.ParentId, (node, children) => node.Children = children, node => node.Sort);
        }

        public async Task<bool> UpdatePositionGroupAsync(AddOrUpdatePositionGroupRequest req)
        {
            if (!req.Id.HasValue) throw new ArgumentNullException(nameof(req.Id));
            var entity = await _positionGroupRepository.FindAsync(req.Id) ?? throw new EntityNotFoundException();
            if (req.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为父级");
            }

            entity.GroupName = req.GroupName;
            entity.Remark = req.Remark;
            entity.ParentId = req.ParentId;
            entity.Sort = req.Sort;
            entity.SetTreeProperties(await _positionGroupRepository.FindAsync(req.ParentId));

            await _positionGroupRepository.UpdateAsync(entity);
            return true;
        }

        public async Task<PositionGroupItem> GetPositionAsync(long id)
        {
            var data = await _positionGroupRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return _mapper.Map<PositionGroupItem>(data);
        }
    }
}