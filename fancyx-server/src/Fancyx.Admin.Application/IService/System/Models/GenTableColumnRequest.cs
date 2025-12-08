using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GenTableColumnRequest : PageSearch
    {
        [Required]
        public long TableId { get; set; }
    }
}
