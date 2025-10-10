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
    public class ProductBrandController : ControllerBase
    {
        public ProductBrandController(IProductBrandService productBrandService)
        {
            ProductBrandService = productBrandService;
        }

        public IProductBrandService ProductBrandService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.ProductBrand.Add")]
        public async Task<AppResponse<bool>> AddProductBrandAsync([FromBody] ProductBrandDto dto)
        {
            await ProductBrandService.AddProductBrandAsync(dto);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.ProductBrand.List")]
        public async Task<AppResponse<PagedResult<ProductBrandListDto>>> GetProductBrandListAsync([FromQuery] ProductBrandQueryDto dto)
        {
            var data = await ProductBrandService.GetProductBrandListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.ProductBrand.Update")]
        public async Task<AppResponse<bool>> UpdateProductBrandAsync([FromBody] ProductBrandDto dto)
        {
            await ProductBrandService.UpdateProductBrandAsync(dto);
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