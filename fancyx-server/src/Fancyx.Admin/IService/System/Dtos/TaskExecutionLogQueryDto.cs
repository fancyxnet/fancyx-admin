using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.IService.System.Dtos
{
    public class TaskExecutionLogQueryDto: PageSearch
    {
        /// <summary>
        /// 任务KEY
        /// </summary>
        [Required]
        public string? TaskKey { get; set; }

        /// <summary>
        /// 执行状态（1:成功 2:失败）
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime[]? ExecutionTimeRange { get; set; }

        /// <summary>
        /// 耗时大于n毫秒
        /// </summary>
        public int? Cost { get; set; }
    }
}
