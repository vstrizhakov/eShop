using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Compositions;

namespace eShop.Catalog.Profiles
{
    public class CompositionProfile : Profile
    {
        public CompositionProfile()
        {
            CreateMap<CreateCompositionRequest, Composition>()
                .ForMember(dest => dest.Id, options => options.Ignore())
                .ForMember(dest => dest.OwnerId, options => options.Ignore())
                .ForMember(dest => dest.CreatedAt, options => options.Ignore())
                .ForMember(dest => dest.DistributionGroupId, options => options.Ignore())
                .ForMember(dest => dest.Images, options => options.Ignore());
        }
    }
}
