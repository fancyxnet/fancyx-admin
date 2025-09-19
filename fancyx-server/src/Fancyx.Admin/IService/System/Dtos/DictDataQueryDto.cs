namespace Fancyx.Admin.IService.System.Dtos
{
    public class DictDataQueryDto : PageSearch
    {
        /// <summary>
        /// 字典标签
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string? DictType { get; set; }
    }
}