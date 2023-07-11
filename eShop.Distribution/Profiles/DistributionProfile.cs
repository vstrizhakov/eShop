using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using eShop.Distribution.Entities;

namespace eShop.Distribution.Profiles
{
    public class DistributionProfile : Profile
    {
        public DistributionProfile()
        {
            CreateMap<DistributionGroup, Models.Distribution>();
            CreateMap<DistributionGroupItem, Models.DistributionItem>()
                .ForMember(dest => dest.DeliveryStatus, options => options.MapFrom(src => src.Status));
            CreateMap<DistributionGroupItemStatus, Models.DeliveryStatus>()
                .ConvertUsingEnumMapping();
        }
    }
}
