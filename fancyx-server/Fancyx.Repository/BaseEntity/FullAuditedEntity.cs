using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.Repository.BaseEntity
{
    public abstract class FullAuditedEntity : AuditedEntity, IDeletionProperty
    {
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        [Column("deleter_id")]
        public Guid? DeleterId { get; set; }

        [Column("deletion_time")]
        public DateTime? DeletionTime { get; set; }

        public void Delete(Guid deleterId)
        {
            IsDeleted = true;
            DeleterId = deleterId;
            DeletionTime = DateTime.Now;
        }
    }
}