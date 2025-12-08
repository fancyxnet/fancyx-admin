using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.Products
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductAttrController : ControllerBase
    {
        public ProductAttrController(IProductAttrService productAttrService)
        {
            ProductAttrService = productAttrService;
        }

        public IProductAttrService ProductAttrService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.ProductAttr.Add")]
        public async Task<AppResponse<bool>> AddProductAttrAsync([FromBody] AddOrUpdateProductAttrRequest req)
        {
            await ProductAttrService.AddProductAttrAsync(req);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.ProductAttr.List")]
        public async Task<AppResponse<PagedResult<ProductAttrItem>>> GetProductAttrListAsync([FromQuery] GetProductAttrListRequest req)
        {
            var data = await ProductAttrService.GetProductAttrListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.ProductAttr.Update")]
        public async Task<AppResponse<bool>> UpdateProductAttrAsync([FromBody] AddOrUpdateProductAttrRequest req)
        {
            await ProductAttrService.UpdateProductAttrAsync(req);
            return Result.Ok();
        }

        [HttpPost("Delete/{id}")]
        [HasPermission("Erp.ProductAttr.Delete")]
        public async Task<AppResponse<bool>> DeleteProductAttrAsync(long id)
        {
            await ProductAttrService.DeleteProductAttrAsync(id);
            return Result.Ok();
        }

        [HttpPost("Value/List")]
        [HasPermission("Erp.ProductAttrValue.Add")]
        public async Task<AppResponse<PagedResult<ProductAttrValueItem>>> GetProductAttrValueListAsync([FromQuery] GetProductAttrValueListRequest req)
        {
            var data = await ProductAttrService.GetProductAttrValueListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Value/Add")]
        [HasPermission("Erp.ProductAttrValue.Add")]
        public async Task<AppResponse<bool>> AddProductAttrValueAsync([FromBody] AddOrUpdateProductAttrValueRequest req)
        {
            await ProductAttrService.AddProductAttrValueAsync(req);
            return Result.Ok();
        }

        [HttpPost("Value/Update")]
        [HasPermission("Erp.ProductAttrValue.Update")]
        public async Task<AppResponse<bool>> UpdateProductAttrValueAsync([FromBody] AddOrUpdateProductAttrValueRequest req)
        {
            await ProductAttrService.UpdateProductAttrValueAsync(req);
            return Result.Ok();
        }

        [HttpPost("Value/Delete")]
        [HasPermission("Erp.ProductAttrValue.Delete")]
        public async Task<AppResponse<bool>> DeleteProductAttrValueAsync(long id)
        {
            await ProductAttrService.DeleteProductAttrValueAsync(id);
            return Result.Ok();
        }
    }
}