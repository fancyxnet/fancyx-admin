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
    }
}