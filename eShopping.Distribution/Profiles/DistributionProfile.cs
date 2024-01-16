using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using eShopping.Distribution.Entities;
using eShopping.Distribution.Models.Distributions;

namespace eShopping.Distribution.Profiles
{
    public class DistributionProfile : Profile
    {
        public DistributionProfile()
        {
            CreateMap<Entities.Distribution, Models.Distributions.Distribution>()
                .ForMember(dest => dest.Recipients, options => options.MapFrom(src => src.Targets));
            CreateMap<DistributionGroup, DistributionRecipient>()
                .ForMember(dest => dest.Client, options => options.MapFrom(src => src.Account));
            CreateMap<Entities.DistributionItem, Models.Distributions.DistributionItem>()
                .ForMember(dest => dest.DeliveryStatus, options => options.MapFrom(src => src.Status));
            CreateMap<DistributionItemStatus, DeliveryStatus>()
                .ConvertUsingEnumMapping();
        }
    }
}
