using Cracker.EfCore.BaseEntity;
using Cracker.IdentityServer.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.EfCore.Entities.Organization
{
    /// <summary>
    /// 职位表
    /// </summary>
    [Table("position")]
    public class Position : AuditedEntity<long>, ITenant
    {
        /// <summary>
        /// 职位编号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        [NotNull]
        [Required]
        [Range(1, int.MaxValue)]
        [Column("level")]
        public int Level { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(512)]
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 职位分组
        /// </summary>
        [Column("group_id")]
        public long? GroupId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}