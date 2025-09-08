using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Core.Interfaces;
using Fancyx.Repository.BaseEntity;
using Fancyx.Repository.Enums;

namespace Fancyx.Repository.Entities.Organization
{
    /// <summary>
    /// 员工表
    /// </summary>
    [Table("org_employee")]
    public class EmployeeDO : FullAuditedEntity, ITenant
    {
        /// <summary>
        /// 工号
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("code")]
        public string? Code { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DefaultValue(0)]
        [Column("sex")]
        public SexType Sex { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(16)]
        [Column("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [StringLength(32)]
        [Column("id_no")]
        public string? IdNo { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        [StringLength(512)]
        [Column("front_id_no_url")]
        public string? FrontIdNoUrl { get; set; }

        /// <summary>
        /// 身份证背面
        /// </summary>
        [StringLength(512)]
        [Column("back_id_no_url")]
        public string? BackIdNoUrl { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 现住址
        /// </summary>
        [StringLength(512)]
        [Column("address")]
        public string? Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(64)]
        [EmailAddress]
        [Column("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        [Column("in_time")]
        public DateTime InTime { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        [Column("out_time")]
        public DateTime? OutTime { get; set; }

        /// <summary>
        /// 状态 1正常2离职
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 关联用户ID
        /// </summary>
        [Column("user_id")]
        public Guid? UserId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Column("dept_id")]
        public Guid? DeptId { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        [Column("position_id")]
        public Guid? PositionId { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }
    }
}