namespace Fancyx.Admin.IService.System.Dtos
{
    public class UserQueryDto : PageSearch
    {
        public string? UserName { get; set; }

        public Guid? DeptId { get; set; }
    }
}