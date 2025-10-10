using AutoMapper;
using Fancyx.EfCore;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.EfCore.Repositories;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.Products
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;

        public ProductBrandService(IRepository<ProductBrand> productBrandRepository, IMapper mapper, ProductRepository productRepository)
        {
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task AddProductBrandAsync(ProductBrandDto dto)
        {
            var codeIsExist = await _productBrandRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            var productBrand = _mapper.Map<ProductBrand>(dto);
            await _productBrandRepository.InsertAsync(productBrand);
        }

        public async Task DeleteProductBrandAsync(long id)
        {
            var isUsed = await _productRepository.AnyAsync(x => x.BrandId == id);
            if (isUsed)
            {
                throw new BusinessException("品牌已被使用");
            }
            await _productBrandRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductBrandListDto>> GetProductBrandListAsync(ProductBrandQueryDto dto)
        {
            var data = await _productBrandRepository.GetQueryable().WhereIf(!string.IsNullOrEmpty(dto.Name), x => x.Name.StartsWith(dto.Name!))
                .PagedAsync(dto.Current, dto.PageSize);
            return new PagedResult<ProductBrandListDto>(data.Total, _mapper.Map<List<ProductBrandListDto>>(data.Items));
        }

        public async Task UpdateProductBrandAsync(ProductBrandDto dto)
        {
            var productBrand = await _productBrandRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = productBrand.Code != dto.Code && await _productBrandRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编码已存在");
            }
            _mapper.Map(dto, productBrand);
            await _productBrandRepository.UpdateAsync(productBrand);
        }
    }
}