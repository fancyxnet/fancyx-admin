using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.Products
{
    internal class ProductCategoryService : IProductCategoryService
    {
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IMapper _mapper;

        public ProductCategoryService(IRepository<ProductCategory> productCategoryRepository, IMapper mapper)
        {
            _productCategoryRepository = productCategoryRepository;
            _mapper = mapper;
        }

        public async Task AddProductCategoryAsync(ProductCategoryDto dto)
        {
            var codeIsExist = await _productCategoryRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productCategory = new ProductCategory()
            {
                Code = dto.Code,
                Name = dto.Name,
                Remark = dto.Remark,
                IsEnabled = dto.IsEnabled
            };
            await _productCategoryRepository.InsertAsync(productCategory);
        }

        public async Task DeleteProductCategoryAsync(long id)
        {
            await _productCategoryRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductCategoryListDto>> GetProductCategoryListAsync(ProductCategoryQueryDto dto)
        {
            var resp = await _productCategoryRepository.GetQueryable()
                .WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ProductCategoryListDto>(resp.Total, _mapper.Map<List<ProductCategoryListDto>>(resp.Items));
        }

        public async Task UpdateProductCategoryAsync(ProductCategoryDto dto)
        {
            var productCategory = await _productCategoryRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productCategory.Code != dto.Code && await _productCategoryRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            productCategory.Code = dto.Code;
            productCategory.Name = dto.Name;
            productCategory.Remark = dto.Remark;
            productCategory.IsEnabled = dto.IsEnabled;
            await _productCategoryRepository.UpdateAsync(productCategory);
        }
    }
}