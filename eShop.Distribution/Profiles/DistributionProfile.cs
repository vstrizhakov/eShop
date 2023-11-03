using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using eShop.Distribution.Models.Distributions;

namespace eShop.Distribution.Profiles
{
    public class DistributionProfile : Profile
    {
        public DistributionProfile()
        {
            CreateMap<Entities.Distribution, Models.Distributions.Distribution>()
                .ForMember(dest => dest.Recipients, options => options.MapFrom(src => src.Items.GroupBy(e => e.Account)));
            CreateMap<IGrouping<Entities.Account, Entities.DistributionItem>, DistributionRecipient>()
                .ForMember(dest => dest.Client, options => options.MapFrom(src => src.Key))
                .ForMember(dest => dest.Items, options => options.MapFrom(src => src.AsEnumerable()));
            CreateMap<Entities.DistributionItem, DistributionItem>()
                .ForMember(dest => dest.DeliveryStatus, options => options.MapFrom(src => src.Status));
            CreateMap<Entities.DistributionItemStatus, DeliveryStatus>()
                .ConvertUsingEnumMapping();
        }
    }
}
