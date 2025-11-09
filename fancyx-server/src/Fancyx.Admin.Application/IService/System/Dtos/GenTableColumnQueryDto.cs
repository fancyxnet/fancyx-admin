using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenTableColumnQueryDto : PageSearch
    {
        [Required]
        public long TableId { get; set; }
    }
}
