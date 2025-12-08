namespace Fancyx.Admin.Application.IService.System.Models
{
    public class SaveGenTableInfoRequest
    {
        /// <summary>
        /// 编号
        /// </summary>
        public long TableId { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string? TableComment { get; set; }

        /// <summary>
        /// 实体类名称
        /// </summary>
        public string? ClassName { get; set; }

        /// <summary>
        /// 使用的模板（crud单表操作 tree树表操作）
        /// </summary>
        public string? TplCategory { get; set; }

        /// <summary>
        /// 生成命名空间路径
        /// </summary>
        public string? NamespaceName { get; set; }

        /// <summary>
        /// 生成模块名
        /// </summary>
        public string? ModuleName { get; set; }

        /// <summary>
        /// 生成业务名
        /// </summary>
        public string? BusinessName { get; set; }

        /// <summary>
        /// 生成功能名
        /// </summary>
        public string? FunctionName { get; set; }

        /// <summary>
        /// 生成代码方式（0zip压缩包 1自定义路径）
        /// </summary>
        public string? GenType { get; set; }

        /// <summary>
        /// 生成路径
        /// </summary>
        public string? GenPath { get; set; }

        /// <summary>
        /// 其它生成选项
        /// </summary>
        public string? Options { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; }
    }
}
