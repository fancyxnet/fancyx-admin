namespace Fancyx.EfCore.BaseEntity
{
    public interface IHasDeletionFlagProperty
    {
        bool IsDeleted { get; set; }
    }
}
