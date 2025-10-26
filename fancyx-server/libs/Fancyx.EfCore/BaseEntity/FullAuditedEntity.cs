using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.EfCore.BaseEntity
{
    public abstract class FullAuditedEntity<TKey> : AuditedEntity<TKey>, IHasCreationProperty, IHasModificationProperty, IHasDeletionProperty
    {
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("deleter_id")]
        public long? DeleterId { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public void Delete(long deleterId)
        {
            IsDeleted = true;
            DeleterId = deleterId;
            DeletionTime = DateTime.Now;
        }
    }
}