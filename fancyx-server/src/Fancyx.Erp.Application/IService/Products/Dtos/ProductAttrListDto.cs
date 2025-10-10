namespace Fancyx.Erp.Application.IService.Products.Dtos
{
    public class ProductAttrListDto
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Remark { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// 多选/单选/手动录入
        /// </summary>
        public string InputType { get; set; } = null!;

        /// <summary>
        /// 指定分类/所有分类
        /// </summary>
        public int AttrType { get; set; }

        public long? CategoryId { get; set; }
    }
}