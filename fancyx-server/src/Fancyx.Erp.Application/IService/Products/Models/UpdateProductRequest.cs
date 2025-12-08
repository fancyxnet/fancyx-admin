namespace Fancyx.Erp.Application.IService.Products.Models
{
    public class UpdateProductRequest
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public string SkuCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Remark { get; set; }
        public bool IsEnabled { get; set; }
        public long BrandId { get; set; }
        public long CategoryId { get; set; }
        public List<ProductBindAttrValueInfo>? Attrs { get; set; }
        public int Unit { get; set; }
    }
}