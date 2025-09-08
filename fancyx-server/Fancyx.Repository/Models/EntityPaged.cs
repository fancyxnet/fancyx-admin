namespace Fancyx.Repository.Models
{
    public class EntityPaged<T>
    {
        public int Total { get; set; }

        public List<T> Items { get; set; } = [];
    }
}