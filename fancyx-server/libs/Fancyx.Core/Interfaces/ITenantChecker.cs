namespace Fancyx.Core.Interfaces
{
    public interface ITenantChecker
    {
        /// <summary>
        /// 是否存在租户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task<bool> ExistTenantAsync(string tenantId);

        /// <summary>
        /// 通过域名获取租户
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        Task<string> GetTenantByDomainAsync(string domain);
    }
}