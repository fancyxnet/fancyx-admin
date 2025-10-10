using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using Fancyx.EfCore.BaseEntity;

namespace Fancyx.Erp.EfCore.Entites
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [Table("customer")]
    public class Customer : FullAuditedEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Column("code")]
        [NotNull, Required]
        public string Code { get; set; } = null!;

        /// <summary>
        /// 简码
        /// </summary>
        [Column("code_slim")]
        [NotNull, Required]
        public string? CodeSlim { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column("name")]
        [NotNull, Required]
        public string? Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Column("contact_name")]
        public string? ContactName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Column("contact_phone")]
        public string? ContactPhone { get; set; }
    }
}
