namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductDto
    {
        public string Code { get; set; } = null!;
        public string SkuCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Remark { get; set; }
        public bool IsEnabled { get; set; }
        public long BrandId { get; set; }
        public long ClassifyId { get; set; }
    }
}