using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;


namespace Fancyx.Admin.Entities.System
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [Table("sys_login_log")]
    public class LoginLogDO : CreationEntity, ITenant
    {
        /// <summary>
        /// 账号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [StringLength(32)]
        [Column("ip")]
        public string? Ip { get; set; }

        /// <summary>
        /// 登录地址
        /// </summary>
        [StringLength(256)]
        [Column("address")]
        public string? Address { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        [Column("browser")]
        public string? Browser { get; set; }

        /// <summary>
        /// 操作信息
        /// </summary>
        [StringLength(128)]
        [Column("operation_msg")]
        public string? OperationMsg { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        [Column("is_success")]
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        [StringLength(36)]
        [Column("session_id")]
        public string? SessionId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}