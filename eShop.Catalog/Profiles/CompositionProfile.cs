using AutoMapper;

namespace eShop.Catalog.Profiles
{
    public class CompositionProfile : Profile
    {
        public CompositionProfile()
        {
            CreateMap<Models.Compositions.CreateCompositionRequest, Entities.Composition>()
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.OwnerId, options => options.Ignore())
                .ForMember(dest => dest.CreatedAt, options => options.Ignore())
                .ForMember(dest => dest.DistributionGroupId, options => options.Ignore())
                .ForMember(dest => dest.Images, options => options.Ignore());
            CreateMap<Entities.Composition, Models.Compositions.Composition>()
                .ForMember(dest => dest.DistributionId, options => options.MapFrom(src => src.DistributionGroupId));
        }
    }
}
