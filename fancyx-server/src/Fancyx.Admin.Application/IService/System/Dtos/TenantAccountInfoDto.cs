using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class TenantAccountInfoDto
    {

        public string? RoleName { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}
