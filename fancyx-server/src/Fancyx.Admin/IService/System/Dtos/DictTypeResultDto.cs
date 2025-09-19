using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.IService.System.Dtos;

public class DictTypeResultDto
{
    public string? Name { get; set; }

    [NotNull]
    [Required]
    public Guid Id { get; set; }

    [NotNull]
    [Required]
    public bool IsEnabled { get; set; }

    public string? DictType { get; set; }

    public string? Remark { get; set; }

    public DateTime CreationTime { get; set; }
}