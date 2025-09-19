namespace Fancyx.Admin.IService.System.Dtos
{
    public class ScheduledTaskQueryDto: PageSearch
    {
        /// <summary>
        /// 任务KEY
        /// </summary>
        public string? TaskKey { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string? Description { get; set; }
    }
}