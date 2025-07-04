using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.IService.Organization.Dtos
{
    public class DeptDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        public string? Name { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [Required]
        public string? Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 状态：1正常2停用
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public Guid? CuratorId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string? Phone { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}