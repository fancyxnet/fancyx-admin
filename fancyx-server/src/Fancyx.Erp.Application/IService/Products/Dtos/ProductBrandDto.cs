namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductBrandDto
    {
        public long? Id { get; set; }
        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string? Remark { get; set; }
        public bool IsEnabled { get; set; }
    }
}