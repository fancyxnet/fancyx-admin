using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.EfCore.Entities.Gen
{
    /// <summary>
    /// 代码生成业务表字段
    /// </summary>
    public class GenTableColumn
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public long ColumnId { get; set; }

        /// <summary>
        /// 归属表编号
        /// </summary>
        public long TableId { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public string? ColumnName { get; set; }

        /// <summary>
        /// 列描述
        /// </summary>
        public string? ColumnComment { get; set; }

        /// <summary>
        /// 列类型
        /// </summary>
        public string? ColumnType { get; set; }

        /// <summary>
        /// CSharp类型
        /// </summary>
        public string? CsharpType { get; set; }

        /// <summary>
        /// CSharp字段名
        /// </summary>
        public string? CsharpField { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// 是否自增）
        /// </summary>
        public bool IsIncrement { get; set; }

        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 是否为插入字段
        /// </summary>
        public bool IsInsert { get; set; }

        /// <summary>
        /// 是否编辑字段
        /// </summary>
        public bool IsEdit { get; set; }

        /// <summary>
        /// 是否列表字段
        /// </summary>
        public bool IsList { get; set; }

        /// <summary>
        /// 是否查询字段
        /// </summary>
        public bool IsQuery { get; set; }

        /// <summary>
        /// 查询方式（等于、不等于、大于、小于、范围）
        /// </summary>
        public string? QueryType { get; set; }

        /// <summary>
        /// 显示类型（文本框、文本域、下拉框、复选框、单选框、日期控件）
        /// </summary>
        public string? HtmlType { get; set; }

        /// <summary>
        /// 字典类型
        /// </summary>
        public string? DictType { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
