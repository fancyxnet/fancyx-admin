namespace Fancyx.Admin.EfCore.Models
{
    public class TableInfo
    {
        public string TableName { get; set; } = null!;

        public string? TableComment { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
