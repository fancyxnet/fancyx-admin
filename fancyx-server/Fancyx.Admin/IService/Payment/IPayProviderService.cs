using Fancyx.Admin.IService.Payment.Dtos;
using Fancyx.Core.Interfaces;

namespace Fancyx.Admin.IService.Payment
{
    public interface IPayProviderService : IScopedDependency
    {
        /// <summary>
        /// 新增支付渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AddPayProviderAsync(PayProviderDto dto);

        /// <summary>
        /// 支付渠道分页列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<PagedResult<PayProviderListDto>> GetPayProviderListAsync(PayProviderQueryDto dto);

        /// <summary>
        /// 修改支付渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdatePayProviderAsync(PayProviderDto dto);

        /// <summary>
        /// 删除支付渠道
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePayProviderAsync(Guid id);
    }
}
