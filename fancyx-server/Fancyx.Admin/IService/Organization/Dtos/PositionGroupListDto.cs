namespace Fancyx.Admin.IService.Organization.Dtos
{
    public class PositionGroupListDto
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 分组名
        /// </summary>
        public string? GroupName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 树形路径
        /// </summary>
        public string TreePath { get; set; } = null!;

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }

        public List<PositionGroupListDto>? Children { get; set; }
    }
}