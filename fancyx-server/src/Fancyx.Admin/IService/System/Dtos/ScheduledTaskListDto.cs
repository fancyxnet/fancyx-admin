namespace Fancyx.Admin.IService.System.Dtos
{
    public class ScheduledTaskListDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 任务KEY（唯一标识）
        /// </summary>
        public string? TaskKey { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string? CronExpression { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; } = false;

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}