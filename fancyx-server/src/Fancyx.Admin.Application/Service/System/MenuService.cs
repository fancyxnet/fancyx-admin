using AutoMapper;

using Fancyx.Admin.Application.IService.System;
using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Admin.Application.SharedService;
using Fancyx.Admin.EfCore.Entities.System;
using Fancyx.Admin.EfCore.Enums;
using Cracker.EfCore;
using Cracker.Utils;

using Microsoft.EntityFrameworkCore;
using Cracker.IdentityServer.Abstractions;
using Fancyx.Shared;

namespace Fancyx.Admin.Application.Service.System
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;
        private readonly IdentitySharedService _identitySharedService;
        private readonly ICurrentTenant _currentTenant;

        public MenuService(IRepository<Menu> menuRepository, IMapper mapper, IdentitySharedService identitySharedService, ICurrentTenant currentTenant)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
            _identitySharedService = identitySharedService;
            _currentTenant = currentTenant;
        }

        public async Task<bool> AddMenuAsync(AddOrUpdateMenuRequest req)
        {
            if (req.MenuType == (int)MenuType.Menu && string.IsNullOrWhiteSpace(req.Path))
            {
                throw new BusinessException("菜单的路由不能为空");
            }

            if (req.IsExternal && !StringUtils.IsValidUrlStrict(req.Path))
            {
                throw new BusinessException("外链地址不合法");
            }

            if (req.ParentId.HasValue)
            {
                var parentMenu = await _menuRepository.FindAsync(req.ParentId);
                if (parentMenu != null && parentMenu.MenuType == MenuType.Button)
                {
                    throw new BusinessException(message: "菜单父级不能是按钮");
                }
            }

            var isExist = await _menuRepository.AnyAsync(x =>
                x.Path != null && req.Path != null && x.Path.ToLower() == req.Path.ToLower());
            if (isExist)
            {
                throw new BusinessException(message: $"已存在【{req.Path}】菜单路由");
            }

            var entity = _mapper.Map<AddOrUpdateMenuRequest, Menu>(req);
            await _menuRepository.InsertAsync(entity);
            return true;
        }

        public async Task<bool> DeleteMenusAsync(List<long> ids)
        {
            var childIds = await _menuRepository.Where(x => x.ParentId.HasValue && ids.Contains(x.ParentId.Value)).Select(x => x.Id).ToListAsync();
            var isCheckAllChildren = childIds.All(c => ids.Contains(c));
            if (childIds.Count > 0 && !isCheckAllChildren)
            {
                throw new BusinessException("存在子菜单，无法删除");
            }

            await _menuRepository.DeleteAsync(x => ids.Contains(x.Id));
            return true;
        }

        public async Task<List<MenuItem>> GetMenuListAsync(GetMenuListRequest req)
        {
            var isFilter = !string.IsNullOrEmpty(req.Title) || !string.IsNullOrEmpty(req.Path);
            var all = await _menuRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(req.Title),x => !string.IsNullOrEmpty(x.Title) && x.Title.Contains(req.Title!))
                .WhereIf(!string.IsNullOrEmpty(req.Path), x => !string.IsNullOrEmpty(x.Path) && x.Path.Contains(req.Path!))
                .ToListAsync();
            if (isFilter) return _mapper.Map<List<Menu>, List<MenuItem>>(all);

            return CollectionUtils.BuildTree(all, g => new MenuItem
            {
                Id = g.Id,
                Title = g.Title,
                Icon = g.Icon,
                Path = g.Path,
                MenuType = (int)g.MenuType,
                Permission = g.Permission,
                ParentId = g.ParentId,
                Sort = g.Sort,
                Display = g.Display,
                Component = g.Component,
                IsExternal = g.IsExternal,
                KeepAlive = g.KeepAlive,
            }, g => g.Id, g => g.ParentId, (node, children) => node.Children = children, node => node.Sort);
        }

        public async Task<(string[] keys, List<MenuOptionTree> tree)> GetMenuOptionsAsync(bool onlyMenu,
            string? keyword, bool noTenantMenuFilter = false)
        {
            var query = _menuRepository.GetQueryable();
            if (MultiTenancyVars.IsEnabled && !noTenantMenuFilter)
            {
                var tenantMenuIds = await _identitySharedService.GetTenantMenusAsync(_currentTenant.TenantId!);
                query = query.Where(x => tenantMenuIds.Contains(x.Id));
            }
            var isKeywordSearch = !string.IsNullOrEmpty(keyword);
            var all = await query
                .WhereIf(onlyMenu, x => x.MenuType == MenuType.Folder || x.MenuType == MenuType.Menu)
                .WhereIf(isKeywordSearch, x => x.Title != null && x.Title.Contains(keyword!)).ToListAsync();
            var keys = all.Select(x => x.Id.ToString()).ToArray();

            if (isKeywordSearch)
            {
                var list = all.Select(x => new MenuOptionTree()
                { Key = x.Id.ToString(), Title = x.Title, MenuType = (int)x.MenuType }).ToList();
                return (keys, list);
            }

            var top = all.Where(x => !x.ParentId.HasValue && (x.MenuType == MenuType.Folder || x.MenuType == MenuType.Menu))
                .OrderBy(x => x.Sort).ToList();
            var topMap = new List<MenuOptionTree>();
            foreach (var item in top)
            {
                topMap.Add(new MenuOptionTree
                {
                    Key = item.Id.ToString(),
                    Title = item.Title,
                    Children = getChildren(item.Id),
                    MenuType = (int)item.MenuType
                });
            }

            List<MenuOptionTree>? getChildren(long currentId)
            {
                var children = all.Where(x => x.ParentId == currentId).OrderBy(x => x.Sort).ToList();
                if (children.Count == 0) return null;
                var childrenMap = new List<MenuOptionTree>();
                foreach (var item in children)
                {
                    childrenMap.Add(new MenuOptionTree
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

        public async Task<bool> UpdateMenuAsync(AddOrUpdateMenuRequest req)
        {
            if (req.MenuType == (int)MenuType.Menu && string.IsNullOrWhiteSpace(req.Path))
            {
                throw new BusinessException("菜单的路由不能为空");
            }

            if (req.IsExternal && !StringUtils.IsValidUrlStrict(req.Path))
            {
                throw new BusinessException("外链地址不合法");
            }

            if (req.ParentId.HasValue)
            {
                var parentMenu = await _menuRepository.FindAsync(req.ParentId);
                if (parentMenu != null && parentMenu.MenuType == MenuType.Button)
                {
                    throw new BusinessException(message: "菜单父级不能是按钮");
                }
            }

            var isExist = await _menuRepository.AnyAsync(x =>
                x.Path != null && req.Path != null && x.Path.ToLower() == req.Path.ToLower());
            var entity = await _menuRepository.Where(x => x.Id == req.Id).FirstAsync() ??
                         throw new BusinessException("数据不存在");
            if (isExist && entity.Path != null && req.Path!.ToLower() != entity.Path.ToLower())
            {
                throw new BusinessException(message: $"已存在【{req.Path}】菜单路由");
            }

            if (req.ParentId == entity.Id)
            {
                throw new BusinessException(message: "不能选择自己为父级");
            }

            bool updatePermission = req.Permission != entity.Permission;

            entity.Title = req.Title;
            entity.Icon = req.Icon;
            entity.Path = req.Path;
            entity.MenuType = (MenuType)req.MenuType;
            entity.Permission = req.Permission;
            entity.ParentId = req.ParentId;
            entity.Sort = req.Sort;
            entity.Display = req.Display;
            entity.Component = req.Component;
            entity.IsExternal = req.IsExternal;
            entity.KeepAlive = req.KeepAlive;
            await _menuRepository.UpdateAsync(entity);

            return true;
        }

        public async Task<MenuItem> GetMenuAsync(long id)
        {
            var data = await _menuRepository.FindAsync(id) ?? throw new EntityNotFoundException();
            return _mapper.Map<MenuItem>(data);
        }
    }
}