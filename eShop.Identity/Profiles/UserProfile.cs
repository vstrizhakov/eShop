using eShop.Identity.Entities;
using eShop.Identity.Models;

namespace eShop.Identity.Profiles
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpRequest, User>()
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.NormalizedUserName, options => options.Ignore())
                .ForMember(dest => dest.NormalizedEmail, options => options.Ignore())
                .ForMember(dest => dest.EmailConfirmed, options => options.Ignore())
                .ForMember(dest => dest.PasswordHash, options => options.Ignore())
                .ForMember(dest => dest.SecurityStamp, options => options.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, options => options.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, options => options.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, options => options.Ignore())
                .ForMember(dest => dest.LockoutEnd, options => options.Ignore())
                .ForMember(dest => dest.LockoutEnabled, options => options.Ignore())
                .ForMember(dest => dest.AccessFailedCount, options => options.Ignore())
                .ForMember(dest => dest.AccountId, options => options.Ignore())
                .ForMember(dest => dest.Id, options => options.Ignore());
        }
    }
}
