namespace Fancyx.Admin.IService.System.Dtos
{
    public class MenuOptionTreeDto
    {
        public string? Key { get; set; }

        public string? Title { get; set; }
        public int MenuType { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public List<MenuOptionTreeDto>? Children { get; set; }
    }
}