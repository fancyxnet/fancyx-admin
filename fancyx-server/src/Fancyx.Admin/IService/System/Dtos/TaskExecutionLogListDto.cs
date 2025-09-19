namespace Fancyx.Admin.IService.System.Dtos
{
    public class TaskExecutionLogListDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 任务KEY
        /// </summary>
        public string? TaskKey { get; set; }

        /// <summary>
        /// 执行状态（1:成功 2:失败）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 执行结果或错误信息
        /// </summary>
        public string? Result { get; set; }

        /// <summary>
        /// 服务器节点标识
        /// </summary>
        public string? NodeId { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 耗时（单位毫秒）
        /// </summary>
        public int Cost { get; set; }
    }
}
