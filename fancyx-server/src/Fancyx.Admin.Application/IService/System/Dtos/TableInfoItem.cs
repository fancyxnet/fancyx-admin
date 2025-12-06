namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class TableInfoItem
    {
        public string? TableName { get; set; }

        public string? TableComment { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
