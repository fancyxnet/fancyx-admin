using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Dtos;
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
        public async Task<AppResponse<bool>> AddSupplierAsync([FromBody] SupplierDto dto)
        {
            await SupplierService.AddSupplierAsync(dto);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Erp.Supplier.List")]
        public async Task<AppResponse<PagedResult<SupplierListDto>>> GetSupplierListAsync([FromQuery] SupplierQueryDto dto)
        {
            var data = await SupplierService.GetSupplierListAsync(dto);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Erp.Supplier.Update")]
        public async Task<AppResponse<bool>> UpdateSupplierAsync([FromBody] SupplierDto dto)
        {
            await SupplierService.UpdateSupplierAsync(dto);
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
