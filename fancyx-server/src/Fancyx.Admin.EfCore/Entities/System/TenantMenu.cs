using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.EfCore.Entities.System
{
    /// <summary>
    /// 租户菜单关联
    /// </summary>
    [PrimaryKey(nameof(TenantId), nameof(MenuId))]
    public class TenantMenu
    {
        [NotNull, Required]
        public string? TenantId { get; set; } = null!;
        public long MenuId { get; set; }
    }
}
