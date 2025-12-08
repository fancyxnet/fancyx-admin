namespace Fancyx.Admin.Application.IService.System.Models
{
    public class MenuOptionTree
    {
        public string? Key { get; set; }

        public string? Title { get; set; }
        public int MenuType { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<MenuOptionTree>? Children { get; set; }
    }
}