using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.EfCore.BaseEntity
{
    public abstract class Entity
    {
        [Key]
        [NotNull]
        [Required]
        [Column("id")]
        public virtual long Id { get; set; }
    }
}