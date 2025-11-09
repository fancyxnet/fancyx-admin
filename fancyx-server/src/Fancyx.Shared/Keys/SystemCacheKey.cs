namespace Fancyx.Shared.Keys
{
    /// <summary>
    /// 系统模块缓存键
    /// </summary>
    public class SystemCacheKey : CacheKeyBase
    {
        /// <summary>
        /// 所有租户缓存键
        /// </summary>
        public const string AllTenant = "all_tenants";

        /// <summary>
        /// 租户域名缓存键
        /// </summary>
        public const string TenantDomains = "tenant_domains";

        /// <summary>
        /// 系统配置缓存键
        /// </summary>
        public static string SystemConfig = WithTenantPrefix("system_config");

        /// <summary>
        /// 系统配置组缓存键
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static string SystemConfigGroup(string group) => WithTenantPrefix($"system_config_group:{group.ToLower()}");

        /// <summary>
        /// 访问令牌
        /// </summary>
        /// <param name="userId">用户ID(long)</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns></returns>
        public static string AccessToken(long userId, string sessionId) => WithTenantPrefix($"access_token:{userId}:{sessionId}");

        /// <summary>
        /// 访问令牌
        /// </summary>
        /// <param name="userId">用户ID(string)</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns></returns>
        public static string AccessToken(string userId, string sessionId) => WithTenantPrefix($"access_token:{userId}:{sessionId}");

        /// <summary>
        /// 访问令牌
        /// </summary>
        /// <param name="key">用户ID:会话ID</param>
        /// <returns></returns>
        public static string AccessToken(string key) => WithTenantPrefix($"access_token:{key}");

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns></returns>
        public static string RefreshToken(long userId, string sessionId) => WithTenantPrefix($"refresh_token:{userId}:{sessionId}");

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="key">用户ID:会话ID</param>
        /// <returns></returns>
        public static string RefreshToken(string key) => WithTenantPrefix($"refresh_token:{key}");

        /// <summary>
        /// 用户权限信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static string UserPermission(long userId) => WithTenantPrefix($"user_permission:{userId}");

        /// <summary>
        /// 用户权限信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static string UserPermission(string userId) => WithTenantPrefix($"user_permission:{userId}");

        /// <summary>
        /// 用户会话ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string UserSessionId(long userId) => WithTenantPrefix($"user_session:{userId}");

        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static string LoginSmsCode(string phone) => WithTenantPrefix($"admin:login_sms_code:{phone}");
    
        /// <summary>
        /// 用户按部门数据权限KEY
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string UserDeptPower(long UserId) => WithTenantPrefix($"user_dept_power:{UserId}");
    }
}