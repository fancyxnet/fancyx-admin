namespace Fancyx.Admin.EfCore.Models
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; } = null!;
        public string ColumnType { get; set; } = null!;
        public string IsNullable { get; set; } = null!;
        public string ColumnDefault { get; set; } = null!;
        public string ColumnKey { get; set; } = null!;
        public string Extra { get; set; } = null!;
        public string ColumnComment { get; set; } = null!;
    }
}
