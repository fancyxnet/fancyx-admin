using Fancyx.DataAccess.BaseEntity;
using Fancyx.DataAccess.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.DataAccess.Entities.Payment
{
    /// <summary>
    /// 支付渠道
    /// </summary>
    [Table("pay_providers")]
    [Index(nameof(Name), IsUnique = true)]
    public class PayProvider : FullAuditedEntity
    {
        /// <summary>
        /// 启用/禁用状态
        /// </summary>
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 支付类型（支付宝或微信）
        /// </summary>
        [Required]
        [Column("type")]
        public PaymentType Type { get; set; }

        /// <summary>
        /// 支付渠道名称（唯一）
        /// </summary>
        [NotNull]
        [Required]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 支付网关
        /// </summary>
        [Column("gateway")]
        public string? Gateway { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        [Column("app_id")]
        public string? AppId { get; set; }

        /// <summary>
        /// 公钥模式："key"或证书模式："cert"
        /// </summary>
        [Column("sign_mode")]
        public string? SignMode { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        [NotNull]
        [Required]
        [Column("notify_url")]
        public string? NotifyUrl { get; set; }

        /// <summary>
        /// 渠道公钥
        /// </summary>
        [Column("provider_public_key")]
        public string? ProviderPublicKey { get; set; }

        /// <summary>
        /// 应用公钥
        /// </summary>
        [Column("app_public_key")]
        public string? AppPublicKey { get; set; }

        /// <summary>
        /// 应用私钥
        /// </summary>
        [Column("app_private_key")]
        public string? AppPrivateKey { get; set; }

        /// <summary>
        /// 渠道证书
        /// </summary>
        [Column("provider_cert_path")]
        public string? ProviderCertPath { get; set; }

        /// <summary>
        /// 应用证书
        /// </summary>
        [Column("app_cert_path")]
        public string? AppCertPath { get; set; }

        /// <summary>
        /// 根证书
        /// </summary>
        [Column("root_cert_path")]
        public string? RootCertPath { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }
    }
}