namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GetMenuListRequest
    {
        /// <summary>
        /// 显示标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string? Path { get; set; }
    }
}