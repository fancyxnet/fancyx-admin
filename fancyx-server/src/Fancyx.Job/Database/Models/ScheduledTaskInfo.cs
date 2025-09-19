using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Job.Database.Models
{
    public class ScheduledTaskInfo
    {
        /// <summary>
        /// 任务KEY（唯一标识）
        /// </summary>
        [NotNull]
        public string? TaskKey { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        [NotNull]
        public string? CronExpression { get; set; }

        /// <summary>
        /// 任务参数（JSON格式）
        /// </summary>
        public string? JobData { get; set; }
    }
}