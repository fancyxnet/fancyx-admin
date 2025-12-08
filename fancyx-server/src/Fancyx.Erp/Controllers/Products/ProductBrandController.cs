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
    public class ProductBrandController : ControllerBase
    {
        public ProductBrandController(IProductBrandService productBrandService)
        {
            ProductBrandService = productBrandService;
        }

        public IProductBrandService ProductBrandService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.ProductBrand.Add")]
        public async Task<AppResponse<bool>> AddProductBrandAsync([FromBody] AddOrUpdateProductBrand req)
        {
            await ProductBrandService.AddProductBrandAsync(req);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.ProductBrand.List")]
        public async Task<AppResponse<PagedResult<ProductBrandItem>>> GetProductBrandListAsync([FromQuery] GetProductBrandListRequest req)
        {
            var data = await ProductBrandService.GetProductBrandListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.ProductBrand.Update")]
        public async Task<AppResponse<bool>> UpdateProductBrandAsync([FromBody] AddOrUpdateProductBrand req)
        {
            await ProductBrandService.UpdateProductBrandAsync(req);
            return Result.Ok();
        }

        [HttpPost("Delete/{id}")]
        [HasPermission("Erp.ProductBrand.Delete")]
        public async Task<AppResponse<bool>> DeleteProductBrandAsync(long id)
        {
            await ProductBrandService.DeleteProductBrandAsync(id);
            return Result.Ok();
        }
    }
}