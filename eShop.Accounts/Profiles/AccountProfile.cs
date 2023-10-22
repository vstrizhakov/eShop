namespace eShop.Accounts.Profiles
{
    public class AccountProfile : AutoMapper.Profile
    {
        public AccountProfile()
        {
            CreateMap<Entities.Account, Messaging.Models.Account>();
        }
    }
}
