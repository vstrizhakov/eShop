using eShop.Identity.DbContexts;
using eShop.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _dbContext;

        public UserRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber);
            return user;
        }
    }
}
