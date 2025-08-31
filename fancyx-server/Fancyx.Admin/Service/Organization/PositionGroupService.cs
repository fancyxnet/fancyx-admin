using Fancyx.Admin.Entities.Organization;
using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Helpers;
using Fancyx.Repository;

namespace Fancyx.Admin.Service.Organization
{
    public class PositionGroupService : IPositionGroupService
    {
        private readonly IRepository<PositionGroupDO> _positionGroupRepository;
        private readonly IRepository<PositionDO> _positionRepository;

        public PositionGroupService(IRepository<PositionGroupDO> positionGroupRepository,
            IRepository<PositionDO> positionRepository)
        {
            _positionGroupRepository = positionGroupRepository;
            _positionRepository = positionRepository;
        }

        public async Task<bool> AddPositionGroupAsync(PositionGroupDto dto)
        {
            var entity = AutoMapperHelper.Instance.Map<PositionGroupDto, PositionGroupDO>(dto);
            entity.ParentId = dto.ParentId;
            if (entity.ParentId.HasValue)
            {
                var all = await _positionGroupRepository.Select.ToListAsync();
                entity.ParentIds = GetParentIds(all, entity.ParentId.Value);
            }

            await _positionGroupRepository.InsertAsync(entity);
            return true;
        }

        public string GetParentIds(List<PositionGroupDO> all, Guid id)
        {
            var parentId = all.Find(x => x.Id == id)?.ParentId;
            if (parentId == null) return id.ToString();
            return GetParentIds(all, parentId.Value) + "," + id;
        }

        public async Task<bool> DeletePositionGroupAsync(Guid id)
        {
            var hasChildren = await _positionGroupRepository.Select.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
            {
                throw new BusinessException("存在子分组，不能删除");
            }

            var hasPositions = await _positionRepository.Select.AnyAsync(x => x.GroupId == id);
            if (hasPositions)
            {
                throw new BusinessException(message: "分组下有职位，不能删除");
            }

            await _positionGroupRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<List<PositionGroupListDto>> GetPositionGroupListAsync(PositionGroupQueryDto dto)
        {
            var rawTree = await _positionGroupRepository.Select
                .WhereIf(!string.IsNullOrEmpty(dto.GroupName), x => x.GroupName.Contains(dto.GroupName!))
                .OrderBy(x => x.Sort)
                .ToTreeListAsync();

            return AutoMapperHelper.Instance.Map<List<PositionGroupDO>, List<PositionGroupListDto>>(rawTree);
        }

        public async Task<bool> UpdatePositionGroupAsync(PositionGroupDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _positionGroupRepository.Where(x => x.Id == dto.Id).FirstAsync();
            if (dto.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为父级");
            }

            entity.GroupName = dto.GroupName;
            entity.Remark = dto.Remark;
            entity.ParentId = dto.ParentId;
            entity.Sort = dto.Sort;
            if (entity.ParentId.HasValue)
            {
                var all = await _positionGroupRepository.Select.ToListAsync();
                entity.ParentIds = GetParentIds(all, entity.ParentId.Value);
            }

            await _positionGroupRepository.UpdateAsync(entity);
            return true;
        }
    }
}