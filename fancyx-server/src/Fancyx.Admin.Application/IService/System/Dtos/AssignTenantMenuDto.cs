namespace Fancyx.Admin.Application.IService.System.Dtos
{
    public class AssignTenantMenuDto
    {
        public long TenantId { get; set; }

        public long[]? MenuIds { get; set; }
    }
}
