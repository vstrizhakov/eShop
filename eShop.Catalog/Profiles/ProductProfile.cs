using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Products;

namespace eShop.Catalog.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductRequest, Product>()
                .ForMember(dest => dest.Prices, options =>
                    options.MapFrom(src => new List<Models.Products.ProductPrice>() { src.Price }))
                .ForMember(dest => dest.Images, options => options.Ignore());
            CreateMap<Models.Products.ProductPrice, Entities.ProductPrice>()
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Price));
        }
    }
}
