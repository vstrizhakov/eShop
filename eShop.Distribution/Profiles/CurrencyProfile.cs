using AutoMapper;
using eShop.Messaging.Models;

namespace eShop.Distribution.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, Entities.Currency>();
            CreateMap<Entities.Currency, Currency>();
        }
    }
}
