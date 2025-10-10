using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.EfCore.BaseEntity
{
    public abstract class CreationEntity : Entity
    {
        [Column("creator_id")]
        public long? CreatorId { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }
    }
}