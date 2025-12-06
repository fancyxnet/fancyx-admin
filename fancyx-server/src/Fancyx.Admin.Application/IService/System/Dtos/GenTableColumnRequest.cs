using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class GenTableColumnRequest : PageSearch
    {
        [Required]
        public long TableId { get; set; }
    }
}
