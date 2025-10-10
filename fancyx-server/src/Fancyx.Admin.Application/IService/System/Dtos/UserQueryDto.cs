namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class UserQueryDto : PageSearch
    {
        public string? UserName { get; set; }

        public long? DeptId { get; set; }
    }
}