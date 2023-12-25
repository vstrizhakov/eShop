using AutoMapper;
using eShop.Messaging.Contracts;

namespace eShop.Distribution.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, Entities.Currency>();
            CreateMap<Entities.Currency, Currency>();
            CreateMap<Entities.EmbeddedCurrency, Currency>();
        }
    }
}
