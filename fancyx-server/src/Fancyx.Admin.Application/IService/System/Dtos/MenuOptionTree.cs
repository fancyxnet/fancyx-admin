namespace Fancyx.Admin.Application.IService.System.Dtos
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