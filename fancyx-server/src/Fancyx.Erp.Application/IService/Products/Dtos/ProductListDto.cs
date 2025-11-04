namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductListDto
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;

        public string SkuCode { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string? Remark { get; set; }

        public bool IsEnabled { get; set; }

        public string? Brand { get; set; }

        public string? Category { get; set; }

        public string? Unit { get; set; }
        public string? UnitText { get; set; }
    }
}