using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Models;
using Fancyx.Erp.EfCore.Models;
using Fancyx.Shared.Models;
using Fancyx.Shared.WebApi.Attributes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fancyx.Erp.Controllers.Products
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        public IProductService ProductService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.Product.Add")]
        public async Task<AppResponse<bool>> AddProductAsync([FromBody] AddOrUpdateProductRequest req)
        {
            await ProductService.AddProductAsync(req);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.Product.List")]
        public async Task<AppResponse<PagedResult<ProductItem>>> GetProductListAsync([FromQuery] GetProductListRequest req)
        {
            var data = await ProductService.GetProductListAsync(req);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.Product.Update")]
        public async Task<AppResponse<bool>> UpdateProductAsync([FromBody] UpdateProductRequest req)
        {
            await ProductService.UpdateProductAsync(req);
            return Result.Ok();
        }

        [HttpPost("Delete/{id}")]
        [HasPermission("Erp.Product.Delete")]
        public async Task<AppResponse<bool>> DeleteProductAsync(long id)
        {
            await ProductService.DeleteProductAsync(id);
            return Result.Ok();
        }
    }
}
