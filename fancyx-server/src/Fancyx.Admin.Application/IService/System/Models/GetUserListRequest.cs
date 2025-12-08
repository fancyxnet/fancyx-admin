namespace Fancyx.Admin.Application.IService.System.Models
{
    public class GetUserListRequest : PageSearch
    {
        public string? UserName { get; set; }

        public long? DeptId { get; set; }
    }
}