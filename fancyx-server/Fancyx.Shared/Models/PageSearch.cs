using Fancyx.Shared.Interfaces;

namespace Fancyx.Shared.Models
{
    public class PageSearch : IPage
    {
        /// <summary>
        /// 每页n条
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前第n页
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? SortProperty { get; set; }

        /// <summary>
        /// 排序方式，true升序，false降序
        /// </summary>
        public bool IsAsecending { get; set; } = false;
    }
}