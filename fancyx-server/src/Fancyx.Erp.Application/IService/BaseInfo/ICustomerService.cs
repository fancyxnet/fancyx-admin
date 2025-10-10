using Fancyx.Core.Interfaces;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.IService.BaseInfo
{
    public interface ICustomerService : IScopedDependency
    {
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AddCustomerAsync(CustomerDto dto);

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<CustomerListDto>> GetCustomerListAsync(CustomerQueryDto dto);

        Task UpdateCustomerAsync(CustomerDto dto);
        Task DeleteCustomerAsync(long id);
    }
}
