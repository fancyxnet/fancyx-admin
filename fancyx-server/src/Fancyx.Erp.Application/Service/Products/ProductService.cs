using AutoMapper;
using Fancyx.Erp.Application.IService.Products;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.EfCore.Repositories;
using Fancyx.Shared.Exceptions;
using Fancyx.Shared.Models;

namespace Fancyx.Erp.Application.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(ProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var codeIsExist = await _productRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编号已存在");
            }
            var product = _mapper.Map<Product>(dto);
            await _productRepository.InsertAsync(product);
        }

        public async Task DeleteProductAsync(long id)
        {
            await _productRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<PagedResult<ProductListDto>> GetProductListAsync(ProductQueryDto dto)
        {
            var data = await _productRepository.QueryProductListAsync(dto.Current, dto.PageSize, dto.Name);
            return new PagedResult<ProductListDto>(data.Total, _mapper.Map<List<ProductListDto>>(data.Items));
        }

        public async Task UpdateProductAsync(ProductUpdateDto dto)
        {
            var product = await _productRepository.FindAsync(dto.Id) ?? throw new EntityNotFoundException();
            var codeIsExist = product.Code != dto.Code && await _productRepository.AnyAsync(x => x.Code == dto.Code);
            if (codeIsExist)
            {
                throw new BusinessException("编号已存在");
            }
            _mapper.Map(dto, product);
            await _productRepository.UpdateAsync(product);
        }
    }
}