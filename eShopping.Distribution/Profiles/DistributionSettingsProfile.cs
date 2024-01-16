using AutoMapper;
using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Distribution.Profiles
{
    public class DistributionSettingsProfile : Profile
    {
        public DistributionSettingsProfile()
        {
            CreateMap<Entities.DistributionSettings, Models.DistributionSettings>();
            CreateMap<Entities.UserCurrencyRate, CurrencyRate>()
                .ForMember(dest => dest.Currency, options => options.MapFrom(src => src.SourceCurrency));
            CreateMap<Entities.DistributionSettings, DistributionSettings>();
        }
    }
}
