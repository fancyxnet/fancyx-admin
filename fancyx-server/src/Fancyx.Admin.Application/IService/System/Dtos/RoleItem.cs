namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class RoleItem
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string? RoleName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsEnabled { get; set; }
    }
}