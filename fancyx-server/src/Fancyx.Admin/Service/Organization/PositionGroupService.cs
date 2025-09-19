using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Core.Helpers;
using Fancyx.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.Organization
{
    public class PositionGroupService : IPositionGroupService
    {
        private readonly IRepository<PositionGroup> _positionGroupRepository;
        private readonly IRepository<Position> _positionRepository;

        public PositionGroupService(IRepository<PositionGroup> positionGroupRepository,
            IRepository<Position> positionRepository)
        {
            _positionGroupRepository = positionGroupRepository;
            _positionRepository = positionRepository;
        }

        public async Task<bool> AddPositionGroupAsync(PositionGroupDto dto)
        {
            var entity = AutoMapperHelper.Instance.Map<PositionGroupDto, PositionGroup>(dto);
            entity.SetTreeProperties(await _positionGroupRepository.FindAsync(dto.ParentId));

            await _positionGroupRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeletePositionGroupAsync(Guid id)
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

        public async Task<List<PositionGroupListDto>> GetPositionGroupListAsync(PositionGroupQueryDto dto)
        {
            var allNodes = await _positionGroupRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.GroupName), x => x.GroupName.Contains(dto.GroupName!))
                .OrderBy(x => x.Sort)
                .ToDictionaryAsync(k => k.Id);

            var tree = new List<PositionGroupListDto>();
            var nodeDtos = new Dictionary<Guid, PositionGroupListDto>();
            var endDtos = new List<PositionGroupListDto>();

            foreach (var node in allNodes.Values)
            {
                var tmp = AutoMapperHelper.Instance.Map<PositionGroup, PositionGroupListDto>(node);
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

        public async Task<bool> UpdatePositionGroupAsync(PositionGroupDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _positionGroupRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            if (dto.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为父级");
            }

            entity.GroupName = dto.GroupName;
            entity.Remark = dto.Remark;
            entity.ParentId = dto.ParentId;
            entity.Sort = dto.Sort;
            entity.SetTreeProperties(await _positionGroupRepository.FindAsync(dto.ParentId));

            await _positionGroupRepository.UpdateAsync(entity);
            return true;
        }
    }
}