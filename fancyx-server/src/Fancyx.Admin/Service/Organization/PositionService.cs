using AutoMapper;

using Fancyx.Admin.EfCore;
using Fancyx.Admin.EfCore.Entities.Organization;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.IService.Organization;
using Fancyx.Admin.IService.Organization.Dtos;
using Fancyx.Admin.Service.Organization.Models;
using Fancyx.EfCore;
using System.Data;

namespace Fancyx.Admin.Service.Organization
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

        private async Task<List<PosistionLayerNames>> GetPosistionGroupNameAsync(List<Guid> ids)
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

        public async Task<bool> AddPositionAsync(PositionDto dto)
        {
            if (await _positionRepository.AnyAsync(x => x.Code.ToLower() == dto.Code!.ToLower()))
            {
                throw new BusinessException("职位编号已存在");
            }
            var entity = _mapper.Map<PositionDto, Position>(dto);
            await _positionRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeletePositionAsync(Guid id)
        {
            var hasEmployees = await _userRepository.AnyAsync(x => x.PostId == id);
            if (hasEmployees) throw new BusinessException(message: "职位正在使用，不能删除");
            await _positionRepository.DeleteAsync(x => x.Id == id);
            return true;
        }

        public async Task<PagedResult<PositionListDto>> GetPositionListAsync(PositionQueryDto dto)
        {
            var pagedResp = await _positionRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Keyword), x => x.Name.Contains(dto.Keyword!) || x.Code.Contains(dto.Keyword!))
                .WhereIf(dto.Level > 0, x => x.Level == dto.Level)
                .WhereIf(dto.Status > 0, x => x.Status == dto.Status)
                .WhereIf(dto.GroupId.HasValue, x => x.GroupId == dto.GroupId)
                .OrderBy(x => x.Level)
                .OrderBy(x => x.CreationTime)
                .PagedAsync(dto.Current, dto.PageSize);
            var ids = pagedResp.Items.Select(x => x.Id).ToList();
            var list = _mapper.Map<List<Position>, List<PositionListDto>>(pagedResp.Items);
            var names = await GetPosistionGroupNameAsync(ids);
            foreach (var item in list)
            {
                var tmp = names.FirstOrDefault(x => x.Id == item.Id);
                item.LayerName = tmp?.LayerName;
            }
            return new PagedResult<PositionListDto>(pagedResp.Total, list);
        }

        public async Task<bool> UpdatePositionAsync(PositionDto dto)
        {
            if (!dto.Id.HasValue) throw new ArgumentNullException(nameof(dto.Id));
            var entity = await _positionRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            string code = dto.Code!.ToLower();
            if (entity.Code.ToLower() != code && await _positionRepository.AnyAsync(x => x.Code.ToLower() == code))
            {
                throw new BusinessException("职位编号已存在");
            }

            entity.Name = dto.Name;
            entity.Code = dto.Code;
            entity.Level = dto.Level;
            entity.Status = dto.Status;
            entity.Description = dto.Description;
            entity.GroupId = dto.GroupId;
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
    }
}