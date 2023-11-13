using eShop.Identity.Entities;

namespace eShop.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserByIdAsync(string id);
        Task UpdateUserAsync(User user);
    }
}
