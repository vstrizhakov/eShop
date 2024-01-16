using eShopping.Identity.Entities;

namespace eShopping.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User?> GetUserByIdAsync(Guid id);
        Task UpdateUserAsync(User user);
        Task CreateAsync(User user);
        Task DeleteAsync(User user);
    }
}
