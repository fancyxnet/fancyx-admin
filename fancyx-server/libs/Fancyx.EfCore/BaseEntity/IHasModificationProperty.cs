using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.EfCore.BaseEntity
{
    public interface IHasModificationProperty
    {
        [Column("last_modification_time")]
        public DateTime? LastModificationTime { get; set; }

        [AllowNull]
        [Column("last_modifier_id")]
        public long? LastModifierId { get; set; }
    }
}
