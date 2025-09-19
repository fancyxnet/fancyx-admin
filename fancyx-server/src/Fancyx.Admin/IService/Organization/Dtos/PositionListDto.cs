namespace Fancyx.Admin.IService.Organization.Dtos
{
    public class PositionListDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 职位编号
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// 职位名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 职位分组
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 层级分组名
        /// </summary>
        public string? LayerName { get; set; }
    }
}