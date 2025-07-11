﻿using Fancyx.Repository.BaseEntity;
using Fancyx.Core.Interfaces;
using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Fancyx.Logger.Entities
{
    /// <summary>
    /// 异常日志实体类
    /// </summary>
    [Table(Name = "exception_log")]
    public class ExceptionLogDO : CreationEntity, ITenant
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType { get; set; } = null!;

        /// <summary>
        /// 异常消息
        /// </summary>
        [Column(DbType = "text")]
        public string Message { get; set; } = null!;

        /// <summary>
        /// 异常堆栈
        /// </summary>
        [Column(DbType = "text")]
        public string StackTrace { get; set; } = null!;

        /// <summary>
        /// 内部异常信息
        /// </summary>
        [Column(DbType = "text")]
        public string? InnerException { get; set; }

        /// <summary>
        /// 请求路径 (如果是Web请求)
        /// </summary>
        public string? RequestPath { get; set; }

        /// <summary>
        /// 请求方法 (GET, POST等)
        /// </summary>
        public string? RequestMethod { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [StringLength(32)]
        public string? Ip { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [StringLength(512)]
        public string? Browser { get; set; }

        /// <summary>
        /// 跟踪ID (用于关联一次请求的所有日志)
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 是否已处理
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? HandledTime { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>
        public string? HandledBy { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public string? TenantId { get; set; }
    }
}