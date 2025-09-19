using Fancyx.Core.Interfaces;

namespace Fancyx.Job
{
    public interface IJobControl
    {
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="taskKey"></param>
        /// <returns></returns>
        Task StartAsync(string taskKey);

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="taskKey"></param>
        /// <returns></returns>
        Task StopAsync(string taskKey);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cron"></param>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task AddJobAsync(string key, string cron, string? description, bool isActive = false);

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="oldKey"></param>
        /// <param name="key"></param>
        /// <param name="cron"></param>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task UpdateJobAsync(string oldKey, string key, string cron, string? description, bool isActive = false);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteJobAsync(string key);

        /// <summary>
        /// 执行一次
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task TriggerJobAsync(string key);
    }
}