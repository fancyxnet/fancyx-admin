using AutoMapper;
using Fancyx.Erp.Application.IService.Products.Dtos;
using Fancyx.Erp.EfCore.Entites;

namespace Fancyx.Erp.Application.Profiles
{
    public class ProductsAutoMapperProfile : Profile
    {
        public ProductsAutoMapperProfile()
        {
            CreateMap<ProductCategoryListDto, ProductCategory>(MemberList.None);
        }
    }
}