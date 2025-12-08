using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface ICustomerService : IScopedDependency
    {
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task AddCustomerAsync(AddOrUpdateCustomerRequest req);

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<PagedResult<CustomerItem>> GetCustomerListAsync(GetCustomerListRequest req);

        Task UpdateCustomerAsync(AddOrUpdateCustomerRequest req);
        Task DeleteCustomerAsync(long id);
    }
}
