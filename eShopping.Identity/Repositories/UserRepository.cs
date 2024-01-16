using eShopping.Database.Extensions;
using eShopping.Identity.DbContexts;
using eShopping.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            var user = await _context.Users
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return user;
        }

        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _context.Users
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber);
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }
    }
}
