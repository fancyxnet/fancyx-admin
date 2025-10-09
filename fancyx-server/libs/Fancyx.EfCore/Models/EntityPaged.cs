namespace Fancyx.EfCore.Models
{
    public class EntityPaged<T>
    {
        public int Total { get; set; }

        public List<T> Items { get; set; } = [];

        public EntityPaged() { }
        public EntityPaged(int total, List<T> items) 
        {
            Total = total;
            Items = items;
        }
    }
}