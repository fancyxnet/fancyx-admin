using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Models
{
    public class TenantAccountInfo
    {

        public string? RoleName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}
