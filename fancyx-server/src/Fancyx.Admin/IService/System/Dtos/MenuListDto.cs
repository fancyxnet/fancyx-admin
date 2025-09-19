using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class MenuListDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 显示标题/名称
        /// </summary>
        [NotNull]
        public string? Title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 路由/地址
        /// </summary>
        public string? Path { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public int MenuType { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string? Permission { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Display { get; set; }

        /// <summary>
        /// 组件地址
        /// </summary>
        public string? Component { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<MenuListDto>? Children { get; set; }

        /// <summary>
        /// 是否外链
        /// </summary>
        public bool IsExternal { get; set; }
    }
}