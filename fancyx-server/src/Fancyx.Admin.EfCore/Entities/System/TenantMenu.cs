using Microsoft.EntityFrameworkCore;

namespace Fancyx.Admin.EfCore.Entities.System
{
    [PrimaryKey(nameof(TenantId), nameof(MenuId))]
    public class TenantMenu
    {
        public long TenantId { get; set; }
        public long MenuId { get; set; }
    }
}
