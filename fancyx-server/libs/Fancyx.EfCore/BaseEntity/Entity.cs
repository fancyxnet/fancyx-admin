using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.EfCore.BaseEntity
{
    public abstract class Entity<TKey>
    {
        [Key]
        [NotNull]
        [Required]
        [Column("id")]
        public virtual TKey Id { get; set; } = default!;
    }
}