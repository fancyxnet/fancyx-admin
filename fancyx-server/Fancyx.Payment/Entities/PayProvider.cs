using Fancyx.Payment.Enums;
using Fancyx.Repository.BaseEntity;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Payment.Entities
{
    /// <summary>
    /// 支付渠道
    /// </summary>
    [Table(Name = "pay_providers")]
    [Index("uk_pay_provider_name", "Name", true)]
    public class PayProvider : FullAuditedEntity
    {
        /// <summary>
        /// 启用/禁用状态
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 支付类型（支付宝或微信）
        /// </summary>
        [Required]
        [Column(IsNullable = false)]
        public PaymentType Type { get; set; }

        /// <summary>
        /// 支付渠道名称（唯一）
        /// </summary>
        [NotNull]
        [Required]
        [Column(IsNullable = false, StringLength = 128)]
        public string? Name { get; set; }

        /// <summary>
        /// 支付网关
        /// </summary>
        [Column(StringLength = 256)]
        public string? Gateway { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        [Column(StringLength = 256)]
        public string? AppId { get; set; }

        /// <summary>
        /// 公钥模式："key"或证书模式："cert"
        /// </summary>
        [Column(IsNullable = false, StringLength = 16)]
        public string? SignMode { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        [NotNull]
        [Required]
        [Column(StringLength = 256, IsNullable = false)]
        public string? NotifyUrl { get; set; }

        /// <summary>
        /// 渠道公钥
        /// </summary>
        [Column(StringLength = -2)]
        public string? ProviderPublicKey { get; set; }

        /// <summary>
        /// 应用公钥
        /// </summary>
        [Column(StringLength = -2)]
        public string? AppPublicKey { get; set; }

        /// <summary>
        /// 应用私钥
        /// </summary>
        [Column(StringLength = -2)]
        public string? AppPrivateKey { get; set; }

        /// <summary>
        /// 渠道证书
        /// </summary>
        [Column(StringLength = 256)]
        public string? ProviderCertPath { get; set; }

        /// <summary>
        /// 应用证书
        /// </summary>
        [Column(StringLength = 256)]
        public string? AppCertPath { get; set; }

        /// <summary>
        /// 根证书
        /// </summary>
        [Column(StringLength = 256)]
        public string? RootCertPath { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(StringLength = 512)]
        public string? Remark { get; set; }
    }
}