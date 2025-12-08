using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Models;

public class AddOrUpdateDictTypeRequest
{
    /// <summary>
    /// 字典名称
    /// </summary>
    [MaxLength(128)]
    public string? Name { get; set; }

    /// <summary>
    /// 主键ID
    /// </summary>
    public long? Id { get; set; }

    /// <summary>
    /// 是否开启
    /// </summary>
    [NotNull]
    [Required]
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 字典类型
    /// </summary>
    [NotNull]
    [Required]
    [MaxLength(128)]
    public string? DictType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(512)]
    public string? Remark { get; set; }
}