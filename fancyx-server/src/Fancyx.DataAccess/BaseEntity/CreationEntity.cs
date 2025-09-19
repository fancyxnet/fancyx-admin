using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.DataAccess.BaseEntity
{
    public abstract class CreationEntity : Entity
    {
        [Column("creator_id")]
        public Guid? CreatorId { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }
    }
}