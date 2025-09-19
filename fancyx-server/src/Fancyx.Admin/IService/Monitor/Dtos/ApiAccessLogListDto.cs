using System.ComponentModel.DataAnnotations;

using Fancyx.Logger;

namespace Fancyx.Admin.IService.Monitor.Dtos
{
    public class ApiAccessLogListDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// HTTP方法 (GET, POST, PUT等)
        /// </summary>
        public string Method { get; set; } = null!;

        /// <summary>
        /// IP
        /// </summary>
        public string? Ip { get; set; }

        /// <summary>
        /// 请求时间
        /// </summary>
        public DateTime RequestTime { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime? ResponseTime { get; set; }

        /// <summary>
        /// 耗时(毫秒)
        /// </summary>
        public long? Duration { get; set; }

        /// <summary>
        /// 用户ID (可为空，未登录用户)
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 请求体
        /// </summary>
        public string? RequestBody { get; set; }

        /// <summary>
        /// 响应体
        /// </summary>
        public string? ResponseBody { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        public string? Browser { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string? QueryString { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperateType[]? OperateType { get; init; }

        /// <summary>
        /// 操作名称
        /// </summary>
        public string? OperateName { get; init; }
    }
}
