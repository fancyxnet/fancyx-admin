namespace Fancyx.Admin.IService.System.Dtos
{
    public class RoleListDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid Id { get; set; }

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