using Fancyx.Admin.Application.IService.System.Models;
using Cracker.AspNetCore.Interfaces;

namespace Fancyx.Admin.Application.IService.System
{
    public interface ITenantService : IScopedDependency
    {
        /// <summary>
        /// 添加租户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AddTenantAsync(AddOrUpdateTenantRequest req);

        /// <summary>
        /// 租户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<TenantItem>> GetTenantListAsync(GetTenantListRequest req);

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task UpdateTenantAsync(AddOrUpdateTenantRequest req);

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteTenantAsync(string id);

        /// <summary>
        /// 分配租户菜单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AssignTenantMenuAsync(AssignTenantMenuRequest req);

        /// <summary>
        /// 租户菜单ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<long>> GetTenantMenuIdsAsync(string id);

        /// <summary>
        /// 创建租户超管
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<TenantAccountInfo> CreateTenantAccountAsync(CreateTenantAccountRequest req);

        /// <summary>
        /// 租户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TenantDetails> GetTenantAsync(string id);
    }
}