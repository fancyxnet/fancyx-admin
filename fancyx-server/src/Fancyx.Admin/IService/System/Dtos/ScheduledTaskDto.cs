using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class ScheduledTaskDto
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// 任务KEY（唯一标识）
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(100)]
        public string? TaskKey { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [MaxLength(512)]
        public string? Description { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        [NotNull]
        [Required]
        [MaxLength(50)]
        public string? CronExpression { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [NotNull]
        [Required]
        public bool IsActive { get; set; } = false;
    }
}
