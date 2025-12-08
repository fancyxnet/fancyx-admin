using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.BaseInfo
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        public SupplierController(ISupplierService supplierService)
        {
            SupplierService = supplierService;
        }

        public ISupplierService SupplierService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.Supplier.Add")]
        public async Task<AppResponse<bool>> AddSupplierAsync([FromBody] AddOrUpdateSupplierRequest req)
        {
            await SupplierService.AddSupplierAsync(req);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Erp.Supplier.List")]
        public async Task<AppResponse<PagedResult<SupplierItem>>> GetSupplierListAsync([FromQuery] GetSupplierListRequest req)
        {
            var data = await SupplierService.GetSupplierListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Erp.Supplier.Update")]
        public async Task<AppResponse<bool>> UpdateSupplierAsync([FromBody] AddOrUpdateSupplierRequest req)
        {
            await SupplierService.UpdateSupplierAsync(req);
            return Result.Ok();
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission("Erp.Supplier.Delete")]
        public async Task<AppResponse<bool>> DeleteSupplierAsync(long id)
        {
            await SupplierService.DeleteSupplierAsync(id);
            return Result.Ok();
        }
    }
}
