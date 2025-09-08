using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.DataAccess.BaseEntity
{
    public abstract class CreationEntity : Entity
    {
        [NotNull]
        [Required]
        [Column("creator_id")]
        public Guid? CreatorId { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }
    }
}