namespace Fancyx.Admin.IService.Organization.Dtos
{
    public class DeptSimpleInfoDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string? Name { get; set; }
    }
}
