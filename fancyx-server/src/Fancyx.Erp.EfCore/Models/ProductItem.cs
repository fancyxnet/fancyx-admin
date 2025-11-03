namespace Fancyx.Erp.EfCore.Models
{
    public class ProductItem
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;

        public string SkuCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string? Remark { get; set; }

        public bool IsEnabled { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public int Unit { get; set; }
    }
}