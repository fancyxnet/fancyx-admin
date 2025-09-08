using Fancyx.Repository.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Job.Database.Entities
{
    /// <summary>
    /// 定时任务表实体
    /// </summary>
    [Table("scheduled_tasks")]
    [Index(nameof(TaskKey), IsUnique = true)]
    public class ScheduledTaskDO : AuditedEntity
    {
        /// <summary>
        /// 任务KEY（唯一标识）
        /// </summary>
        [NotNull]
        [Column("task_key")]
        public string? TaskKey { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Column("task_description")]
        public string? Description { get; set; }

        /// <summary>
        /// Cron表达式
        [NotNull]
        [Column("cron_expression")]
        public string? CronExpression { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = false;
    }
}