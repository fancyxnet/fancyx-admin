using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.IService.Organization.Dtos
{
    public class PositionGroupDto
    {
        public Guid? Id { get; set; }
        public Guid? ParentId { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Required]
        public string? GroupName { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// ����ֵ
        /// </summary>
        public int Sort { get; set; }
    }
}