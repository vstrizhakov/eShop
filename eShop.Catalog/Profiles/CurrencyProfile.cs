using AutoMapper;

namespace eShop.Catalog.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Entities.Currency, Models.Currencies.Currency>();
            CreateMap<Models.Currencies.CreateCurrencyRequest, Entities.Currency>()
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.CreatedAt, options => options.Ignore());
        }
    }
}
