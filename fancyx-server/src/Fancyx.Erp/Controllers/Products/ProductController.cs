using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
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
        public async Task<AppResponse<bool>> AddProductAsync([FromBody] ProductDto dto)
        {
            await ProductService.AddProductAsync(dto);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.Product.List")]
        public async Task<AppResponse<PagedResult<ProductListDto>>> GetProductListAsync([FromQuery] ProductQueryDto dto)
        {
            var data = await ProductService.GetProductListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.Product.Update")]
        public async Task<AppResponse<bool>> UpdateProductAsync([FromBody] ProductUpdateDto dto)
        {
            await ProductService.UpdateProductAsync(dto);
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
