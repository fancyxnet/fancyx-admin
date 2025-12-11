using Fancyx.Admin.Application.IService.System.Models;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface IMenuService : IScopedDependency
    {
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> AddMenuAsync(AddOrUpdateMenuRequest req);

        /// <summary>
        /// 菜单树形列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<List<MenuItem>> GetMenuListAsync(GetMenuListRequest req);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<bool> UpdateMenuAsync(AddOrUpdateMenuRequest req);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteMenusAsync(long[] ids);

        /// <summary>
        /// 获取菜单组成的选项树
        /// </summary>
        /// <param name="onlyMenu">true:只要目录+菜单</param>
        /// <param name="keyword">关键字筛选</param>
        /// <returns></returns>
        Task<(string[] keys, List<MenuOptionTree> tree)> GetMenuOptionsAsync(bool onlyMenu, string? keyword, bool noTenantMenuFilter = false);

        /// <summary>
        /// 菜单详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MenuItem> GetMenuAsync(long id);
    }
}