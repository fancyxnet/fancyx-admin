using Fancyx.Erp.Application.IService.BaseInfo;
using Fancyx.Erp.Application.IService.BaseInfo.Models;
using Fancyx.Erp.EfCore.Entities;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.BaseInfo
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        public WarehouseController(IWarehouseService warehouseService)
        {
            WarehouseService = warehouseService;
        }

        public IWarehouseService WarehouseService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.Warehouse.Add")]
        public async Task<AppResponse<bool>> AddWarehouseAsync([FromBody] AddOrUpdateWarehouseRequest req)
        {
            await WarehouseService.AddWarehouseAsync(req);
            return Result.Ok();
        }

        [HttpGet("List")]
        [HasPermission("Erp.Warehouse.List")]
        public async Task<AppResponse<PagedResult<WarehouseItem>>> GetWarehouseListAsync([FromQuery] GetWarehouseListRequest req)
        {
            var data = await WarehouseService.GetWarehouseListAsync(req);
            return Result.Data(data);
        }

        [HttpPut("Update")]
        [HasPermission("Erp.Warehouse.Update")]
        public async Task<AppResponse<bool>> UpdateWarehouseAsync([FromBody] AddOrUpdateWarehouseRequest req)
        {
            await WarehouseService.UpdateWarehouseAsync(req);
            return Result.Ok();
        }

        [HttpDelete("Delete/{id}")]
        [HasPermission("Erp.Warehouse.Delete")]
        public async Task<AppResponse<bool>> DeleteWarehouseAsync(long id)
        {
            await WarehouseService.DeleteWarehouseAsync(id);
            return Result.Ok();
        }

        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetWarehouse/{id}")]
        [HasPermission("Erp.Warehouse.List")]
        public async Task<AppResponse<Warehouse>> GetWarehouseAsync(long id)
        {
            var data = await WarehouseService.GetWarehouseAsync(id);
            return Result.Data(data);
        }
    }
}