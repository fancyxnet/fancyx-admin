using AutoMapper;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;
using Fancyx.Erp.EfCore.Models;

namespace Fancyx.Erp.Application.Profiles
{
    public class ProductsAutoMapperProfile : Profile
    {
        public ProductsAutoMapperProfile()
        {
            CreateMap<ProductCategoryListDto, ProductCategory>(MemberList.None);
            CreateMap<ProductDto, Product>(MemberList.None);
            CreateMap<ProductUpdateDto, Product>(MemberList.None);
            CreateMap<ProductItem, ProductListDto>(MemberList.None);
        }
    }
}