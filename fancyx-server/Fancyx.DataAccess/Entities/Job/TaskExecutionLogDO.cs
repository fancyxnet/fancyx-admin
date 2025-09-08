using Fancyx.DataAccess.BaseEntity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancyx.DataAccess.Entities.Job
{
    /// <summary>
    /// 任务执行日志表实体
    /// </summary>
    [Table("task_execution_logs")]
    [Index(nameof(TaskKey))]
    [Index(nameof(ExecutionTime))]
    public class TaskExecutionLogDO : CreationEntity
    {
        /// <summary>
        /// 任务KEY
        /// </summary>
        [Column("task_key")]
        public string? TaskKey { get; set; }

        /// <summary>
        /// 执行状态（1:成功 2:失败）
        /// </summary>
        [Column("status")]
        public int Status { get; set; }

        /// <summary>
        /// 执行结果或错误信息
        /// </summary>
        [Column("result")]
        public string? Result { get; set; }

        /// <summary>
        /// 服务器节点标识
        /// </summary>
        [Column("node_id")]
        public string? NodeId { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        [Column("execution_time")]
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 耗时（单位毫秒）
        /// </summary>
        [Column("cost")]
        public int Cost { get; set; }
    }
}