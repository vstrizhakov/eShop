using AutoMapper;
using eShop.Catalog.Entities;

namespace eShop.Catalog.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Models.Products.CreateProductRequest, Product>()
                .ForMember(dest => dest.Prices, options =>
                    options.MapFrom(src => new List<Models.Products.CreateProductPrice>() { src.Price }))
                .ForMember(dest => dest.Images, options => options.Ignore())
                .ForMember(dest => dest.Id, options => options.Ignore());
            CreateMap<Models.Products.CreateProductPrice, Entities.ProductPrice>()
                .ForMember(dest => dest.Value, options => options.MapFrom(src => src.Price))
                .ForMember(dest => dest.DiscountedValue, options => options.MapFrom(src => src.DiscountedPrice))
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.Currency, options => options.Ignore());
            CreateMap<Product, Models.Products.Product>()
                .ForMember(dest => dest.Price, options => options.MapFrom(src => src.Prices.FirstOrDefault()));
            CreateMap<ProductPrice, Models.Products.ProductPrice>()
                .ForMember(dest => dest.Price, options => options.MapFrom(src => src.Value))
                .ForMember(dest => dest.DiscountedPrice, options => options.MapFrom(src => src.DiscountedValue));
        }
    }
}
