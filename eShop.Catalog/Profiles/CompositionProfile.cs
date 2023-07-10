using AutoMapper;
using eShop.Catalog.Entities;
using eShop.Catalog.Models.Compositions;

namespace eShop.Catalog.Profiles
{
    public class CompositionProfile : Profile
    {
        public CompositionProfile()
        {
            CreateMap<CreateCompositionRequest, Composition>();
        }
    }
}
