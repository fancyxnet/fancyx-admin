using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Admin.EfCore.Enums;
using Fancyx.Shared.EfCore;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("user")]
    public class User : FullAuditedEntity<long>, ITenant
    {
        [Key]
        [NotNull]
        [Required]
        [Column("id")]
        [DataPower(DataPower.UserId)]
        public override long Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [NotNull]
        [StringLength(32)]
        [Column("user_name")]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [NotNull]
        [StringLength(512)]
        [Column("password")]
        public string? Password { get; set; }

        /// <summary>
        /// 密码盐
        /// </summary>
        [NotNull]
        [StringLength(256)]
        [Column("password_salt")]
        public string? PasswordSalt { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(256)]
        [Column("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(64)]
        [Column("nick_name")]
        public string? NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [NotNull]
        [Required]
        [Column("sex")]
        [DefaultValue(0)]
        public SexType Sex { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [Column("dept_id")]
        public long? DeptId { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        [Column("post_id")]
        public long? PostId { get; set; }
    }
}