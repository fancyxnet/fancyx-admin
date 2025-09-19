namespace Fancyx.Admin.IService.Account.Dtos
{
    public class UserAuthInfoDto
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfoDto User { get; set; } = null!;

        /// <summary>
        /// 权限信息
        /// </summary>
        public string[] Permissions { get; set; } = null!;

        /// <summary>
        /// 菜单信息
        /// </summary>
        public List<FrontendMenu> Menus { get; set; } = null!;
    }
}