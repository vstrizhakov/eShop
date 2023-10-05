using AutoMapper;

namespace eShop.Distribution.Profiles
{
    public class DistributionSettingsProfile : Profile
    {
        public DistributionSettingsProfile()
        {
            CreateMap<Entities.DistributionSettings, Models.DistributionSettings>();
            CreateMap<Models.UpdateDistributionSettingsRequest, Entities.DistributionSettings>()
                .ForMember(dest => dest.PreferredCurrencyId, options =>
                {
                    options.Condition(src => src.Currency != null);
                    options.MapFrom(src => src.Currency!.PreferredCurrencyId);
                });
            CreateMap<Entities.CurrencyRate, Messaging.Models.Distribution.CurrencyRate>()
                .ForMember(dest => dest.Currency, options => options.MapFrom(src => src.SourceCurrency));
        }
    }
}
