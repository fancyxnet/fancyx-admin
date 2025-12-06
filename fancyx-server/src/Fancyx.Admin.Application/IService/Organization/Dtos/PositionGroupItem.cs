namespace Fancyx.Admin.Application.IService.Organization.Dtos
{
    public class PositionGroupItem
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }

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
        public long? ParentId { get; set; }

        /// <summary>
        /// 树形路径
        /// </summary>
        public string TreePath { get; set; } = null!;

        /// <summary>
        /// 排序值
        /// </summary>
        public int Sort { get; set; }

        public List<PositionGroupItem>? Children { get; set; }
    }
}