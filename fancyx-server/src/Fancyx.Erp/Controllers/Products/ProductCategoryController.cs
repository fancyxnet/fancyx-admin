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
    public class ProductCategoryController : ControllerBase
    {
        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            ProductCategoryService = productCategoryService;
        }

        public IProductCategoryService ProductCategoryService { get; }

        [HttpPost("Add")]
        [HasPermission("Erp.ProductCategory.Add")]
        public async Task<AppResponse<bool>> AddProductCategoryAsync([FromBody] ProductCategoryDto dto)
        {
            await ProductCategoryService.AddProductCategoryAsync(dto);
            return Result.Ok();
        }

        [HttpPost("List")]
        [HasPermission("Erp.ProductCategory.List")]
        public async Task<AppResponse<PagedResult<ProductCategoryListDto>>> GetProductCategoryListAsync([FromQuery] ProductCategoryQueryDto dto)
        {
            var data = await ProductCategoryService.GetProductCategoryListAsync(dto);
            return Result.Data(data);
        }

        [HttpPost("Update")]
        [HasPermission("Erp.ProductCategory.Update")]
        public async Task<AppResponse<bool>> UpdateProductCategoryAsync([FromBody] ProductCategoryDto dto)
        {
            await ProductCategoryService.UpdateProductCategoryAsync(dto);
            return Result.Ok();
        }

        [HttpPost("Delete/{id}")]
        [HasPermission("Erp.ProductCategory.Delete")]
        public async Task<AppResponse<bool>> DeleteProductCategoryAsync(long id)
        {
            await ProductCategoryService.DeleteProductCategoryAsync(id);
            return Result.Ok();
        }
    }
}