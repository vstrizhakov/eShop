using AutoMapper;

namespace eShop.Distribution.Profiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Messaging.Models.Currency, Entities.Currency>();
        }
    }
}
