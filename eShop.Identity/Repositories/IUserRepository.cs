using eShop.Identity.Entities;

namespace eShop.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    }
}
