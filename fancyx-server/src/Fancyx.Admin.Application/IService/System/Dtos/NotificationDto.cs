using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class NotificationDto
    {
        public long? Id { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        [MaxLength(500)]
        public string? Content { get; set; }

        /// <summary>
        /// 通知用户
        /// </summary>
        [NotNull]
        [Required]
        public long UserId { get; set; }
    }
}