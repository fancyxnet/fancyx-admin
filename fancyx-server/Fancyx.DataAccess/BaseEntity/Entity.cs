using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.DataAccess.BaseEntity
{
    public abstract class Entity
    {
        [Key]
        [NotNull]
        [Required]
        [Column("id")]
        public virtual Guid Id { get; set; }
    }
}