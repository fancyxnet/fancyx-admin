namespace Fancyx.Erp.Application.IService.Products.Models
{
    public class AddOrUpdateProductCategory
    {
        public long? Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Remark { get; set; }
        public bool IsEnabled { get; set; }
    }
}