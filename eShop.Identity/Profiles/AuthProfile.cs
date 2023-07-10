using eShop.Identity.Entities;
using eShop.Identity.Models;

namespace eShop.Identity.Profiles
{
    public class AuthProfile : AutoMapper.Profile
    {
        public AuthProfile()
        {
            CreateMap<SignUpRequest, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.Email));
        }
    }
}
