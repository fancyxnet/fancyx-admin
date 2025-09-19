using Fancyx.Core.Interfaces;
using Fancyx.EfCore.BaseEntity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Fancyx.Admin.EfCore.Enums;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [Table("menu")]
    public class Menu : AuditedEntity, ITenant
    {
        /// <summary>
        /// 显示标题/名称
        /// </summary>
        [NotNull]
        [Required]
        [StringLength(32)]
        [Column("title")]
        public string? Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(64)]
        [Column("icon")]
        public string? Icon { get; set; }

        /// <summary>
        /// 路由/地址
        /// </summary>
        [StringLength(256)]
        [Column("path")]
        public string? Path { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        [StringLength(256)]
        [Column("component")]
        public string? Component { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        [Column("menu_type")]
        public MenuType MenuType { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        [StringLength(128)]
        [Column("permission")]
        public string? Permission { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Column("sort")]
        public int Sort { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [Column("display")]
        public bool Display { get; set; }

        /// <summary>
        /// 角色菜单
        /// </summary>
        public virtual ICollection<RoleMenu>? RoleMenus { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        [Column("tenant_id")]
        public string? TenantId { get; set; }

        /// <summary>
        /// 是否外链
        /// </summary>
        [Column("is_external")]
        public bool IsExternal { get; set; } = false;
    }
}