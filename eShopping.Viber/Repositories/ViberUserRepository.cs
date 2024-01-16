using eShopping.Database.Extensions;
using eShopping.Viber.DbContexts;
using eShopping.Viber.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShopping.Viber.Repositories
{
    public class ViberUserRepository : IViberUserRepository
    {
        private readonly ViberDbContext _context;

        public ViberUserRepository(ViberDbContext context)
        {
            _context = context;
        }

        public async Task CreateViberUserAsync(ViberUser viberUser)
        {
            _context.ViberUsers.Add(viberUser);

            await _context.SaveChangesAsync();
        }

        public async Task<ViberUser?> GetViberUserByAccountIdAsync(Guid accountId)
        {
            var viberUser = await _context.ViberUsers
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.AccountId == accountId);
            return viberUser;
        }

        public async Task<ViberUser?> GetViberUserByExternalIdAsync(string externalId)
        {
            var viberUser = await _context.ViberUsers
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.ExternalId == externalId);
            return viberUser;
        }

        public async Task<ViberUser?> GetViberUserByIdAsync(Guid id)
        {
            var viberUser = await _context.ViberUsers
                .WithDiscriminatorAsPartitionKey()
                .FirstOrDefaultAsync(e => e.Id == id);
            return viberUser;
        }

        public async Task<IEnumerable<ViberUser>> GetViberUsersByIdsAsync(IEnumerable<Guid> ids)
        {
            var viberUsers = await _context.ViberUsers
                .WithDiscriminatorAsPartitionKey()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            return viberUsers;
        }

        public async Task UpdateAccountIdAsync(ViberUser viberUser, Guid accountId)
        {
            viberUser.AccountId = accountId;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateChatSettingsAsync(ViberUser viberUser, bool isEnabled)
        {
            viberUser.ChatSettings.IsEnabled = isEnabled;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateViberUserAsync(ViberUser viberUser)
        {
            await _context.SaveChangesAsync();
        }
    }
}
