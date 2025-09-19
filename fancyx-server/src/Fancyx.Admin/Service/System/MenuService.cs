using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Admin.IService.System;
using Fancyx.Admin.IService.System.Dtos;
using Fancyx.Core.Helpers;
using Fancyx.EfCore;
using Fancyx.Utils;
using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.Service.System
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuService(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<bool> AddMenuAsync(MenuDto dto)
        {
            if (dto.MenuType == (int)MenuType.Menu && string.IsNullOrWhiteSpace(dto.Path))
            {
                throw new BusinessException("菜单的路由不能为空");
            }

            if (dto.IsExternal && !StringUtils.IsValidUrlStrict(dto.Path))
            {
                throw new BusinessException("外链地址不合法");
            }

            if (dto.ParentId.HasValue)
            {
                var parentMenu = await _menuRepository.FindAsync(dto.ParentId);
                if (parentMenu != null && parentMenu.MenuType == MenuType.Button)
                {
                    throw new BusinessException(message: "菜单父级不能是按钮");
                }
            }

            var isExist = await _menuRepository.AnyAsync(x =>
                x.Path != null && dto.Path != null && x.Path.ToLower() == dto.Path.ToLower());
            if (isExist)
            {
                throw new BusinessException(message: $"已存在【{dto.Path}】菜单路由");
            }

            var entity = AutoMapperHelper.Instance.Map<MenuDto, Menu>(dto);
            await _menuRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeleteMenusAsync(Guid[] ids)
        {
            var hasChildren =
                await _menuRepository.AnyAsync(x => x.ParentId.HasValue && ids.Contains(x.ParentId.Value));
            if (hasChildren)
            {
                throw new BusinessException("存在子菜单，无法删除");
            }

            await _menuRepository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }

        public async Task<List<MenuListDto>> GetMenuListAsync(MenuQueryDto dto)
        {
            //是否过滤
            var isFilter = !string.IsNullOrEmpty(dto.Title) || !string.IsNullOrEmpty(dto.Path);
            var all = await _menuRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Title),
                    x => !string.IsNullOrEmpty(x.Title) && x.Title.Contains(dto.Title!))
                .WhereIf(!string.IsNullOrEmpty(dto.Path),
                    x => !string.IsNullOrEmpty(x.Path) && x.Path.Contains(dto.Path!))
                .ToListAsync();
            var top = all.Where(x => isFilter || !x.ParentId.HasValue || x.ParentId == Guid.Empty).OrderBy(x => x.Sort)
                .ToList();
            var topMap = AutoMapperHelper.Instance.Map<List<Menu>, List<MenuListDto>>(top);
            if (isFilter) return topMap;
            foreach (var item in topMap)
            {
                item.Children = getChildren(item.Id);
            }

            List<MenuListDto>? getChildren(Guid currentId)
            {
                var children = all.Where(x => x.ParentId == currentId).OrderBy(x => x.Sort).ToList();
                if (children.Count == 0) return null;
                var childrenMap = AutoMapperHelper.Instance.Map<List<Menu>, List<MenuListDto>>(children);
                foreach (var item in childrenMap)
                {
                    item.Children = getChildren(item.Id);
                }

                return childrenMap;
            }

            return topMap;
        }

        public async Task<(string[] keys, List<MenuOptionTreeDto> tree)> GetMenuOptionsAsync(bool onlyMenu,
            string? keyword)
        {
            var isKeywordSearch = !string.IsNullOrEmpty(keyword);
            var all = await _menuRepository.GetQueryable()
                .WhereIf(onlyMenu, x => x.MenuType == MenuType.Folder || x.MenuType == MenuType.Menu)
                .WhereIf(isKeywordSearch, x => x.Title != null && x.Title.Contains(keyword!)).ToListAsync();
            var keys = all.Select(x => x.Id.ToString()).ToArray();

            if (isKeywordSearch)
            {
                var list = all.Select(x => new MenuOptionTreeDto()
                { Key = x.Id.ToString(), Title = x.Title, MenuType = (int)x.MenuType }).ToList();
                return (keys, list);
            }

            var top = all.Where(x => !x.ParentId.HasValue || x.ParentId == Guid.Empty && x.MenuType == MenuType.Menu)
                .OrderBy(x => x.Sort).ToList();
            var topMap = new List<MenuOptionTreeDto>();
            foreach (var item in top)
            {
                topMap.Add(new MenuOptionTreeDto
                {
                    Key = item.Id.ToString(),
                    Title = item.Title,
                    Children = getChildren(item.Id),
                    MenuType = (int)item.MenuType
                });
            }

            List<MenuOptionTreeDto>? getChildren(Guid currentId)
            {
                var children = all.Where(x => x.ParentId == currentId).OrderBy(x => x.Sort).ToList();
                if (children.Count == 0) return null;
                var childrenMap = new List<MenuOptionTreeDto>();
                foreach (var item in children)
                {
                    childrenMap.Add(new MenuOptionTreeDto
                    {
                        Key = item.Id.ToString(),
                        Title = item.Title,
                        Children = getChildren(item.Id),
                        MenuType = (int)item.MenuType
                    });
                }

                return childrenMap;
            }

            return (keys, topMap);
        }

        public async Task<bool> UpdateMenuAsync(MenuDto dto)
        {
            if (dto.MenuType == (int)MenuType.Menu && string.IsNullOrWhiteSpace(dto.Path))
            {
                throw new BusinessException("菜单的路由不能为空");
            }

            if (dto.IsExternal && !StringUtils.IsValidUrlStrict(dto.Path))
            {
                throw new BusinessException("外链地址不合法");
            }

            if (dto.ParentId.HasValue)
            {
                var parentMenu = await _menuRepository.FindAsync(dto.ParentId);
                if (parentMenu != null && parentMenu.MenuType == MenuType.Button)
                {
                    throw new BusinessException(message: "菜单父级不能是按钮");
                }
            }

            var isExist = await _menuRepository.AnyAsync(x =>
                x.Path != null && dto.Path != null && x.Path.ToLower() == dto.Path.ToLower());
            var entity = await _menuRepository.Where(x => x.Id == dto.Id).FirstAsync() ??
                         throw new BusinessException("数据不存在");
            if (isExist && entity.Path != null && dto.Path!.ToLower() != entity.Path.ToLower())
            {
                throw new BusinessException(message: $"已存在【{dto.Path}】菜单路由");
            }

            if (dto.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为父级");
            }

            bool updatePermission = dto.Permission != entity.Permission;

            entity.Title = dto.Title;
            entity.Icon = dto.Icon;
            entity.Path = dto.Path;
            entity.MenuType = (MenuType)dto.MenuType;
            entity.Permission = dto.Permission;
            entity.ParentId = dto.ParentId;
            entity.Sort = dto.Sort;
            entity.Display = dto.Display;
            entity.Component = dto.Component;
            entity.IsExternal = dto.IsExternal;
            await _menuRepository.UpdateAsync(entity);

            return true;
        }
    }
}