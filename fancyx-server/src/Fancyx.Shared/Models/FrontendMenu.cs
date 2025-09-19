namespace Fancyx.Shared.Models
{
    public class FrontendMenu
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 菜单路由
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public string? Component { get; set; }

        /// <summary>
        /// 层级面包屑
        /// </summary>
        public string? LayerName { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<FrontendMenu>? Children { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 是否外链
        /// </summary>
        public bool IsExternal { get; set; }
    }
}