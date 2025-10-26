using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.EfCore.BaseEntity
{
    public interface IHasDeletionProperty : IHasDeletionFlagProperty
    {
        [Column("deleter_id")]
        public long? DeleterId { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        void Delete(long deleterId);
    }
}
