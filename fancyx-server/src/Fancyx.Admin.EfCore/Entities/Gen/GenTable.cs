using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.EfCore.Entities.Gen
{
    /// <summary>
    /// 代码生成业务表
    /// </summary>
    public class GenTable
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public long TableId { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string? TableName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        public string? TableComment { get; set; }

        /// <summary>
        /// 关联子表的表名
        /// </summary>
        public string? SubTableName { get; set; }

        /// <summary>
        /// 子表关联的外键名
        /// </summary>
        public string? SubTableFkName { get; set; }

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
        public string? PackageName { get; set; }

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
        /// 生成功能作者
        /// </summary>
        public string? FunctionAuthor { get; set; }

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
