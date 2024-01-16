using AutoMapper;
using eShopping.Messaging.Contracts;

namespace eShopping.Distribution.Profiles
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
