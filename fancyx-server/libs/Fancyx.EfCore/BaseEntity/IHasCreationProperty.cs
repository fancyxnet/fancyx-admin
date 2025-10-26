using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.EfCore.BaseEntity
{
    public interface IHasCreationProperty
    {
        [Column("creator_id")]
        public long? CreatorId { get; set; }

        [Column("creation_time")]
        public DateTime CreationTime { get; set; }
    }
}
