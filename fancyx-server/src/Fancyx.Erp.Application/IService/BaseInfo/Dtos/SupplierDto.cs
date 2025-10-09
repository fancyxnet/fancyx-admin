namespace Fancyx.Erp.Application.IService.BaseInfo.Dtos
{
    public class SupplierDto
    {
        public long? Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Remark { get; set; }
        public bool IsEnabled { get; set; }
    }
}