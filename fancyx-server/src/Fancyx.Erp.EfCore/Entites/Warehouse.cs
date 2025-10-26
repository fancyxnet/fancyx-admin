using Fancyx.EfCore.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Erp.EfCore.Entites
{
    [Index(nameof(Code), IsUnique = true)]
    public class Warehouse : FullAuditedEntity<long>
    {
        [Column("code")]
        public string Code { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        [Column("remark")]
        public string? Remark { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }
    }
}