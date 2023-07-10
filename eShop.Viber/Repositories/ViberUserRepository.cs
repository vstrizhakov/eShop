using eShop.Viber.DbContexts;
using eShop.Viber.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Viber.Repositories
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

        public async Task<ViberUser?> GetViberUserByExternalIdAsync(string externalId)
        {
            var viberUser = await _context.ViberUsers
                .Include(e => e.ChatSettings)
                .FirstOrDefaultAsync(e => e.ExternalId == externalId);
            return viberUser;
        }

        public async Task<ViberUser?> GetViberUserByIdAsync(Guid id)
        {
            var viberUser = await _context.ViberUsers
                .Include(e => e.ChatSettings)
                .FirstOrDefaultAsync(e => e.Id == id);
            return viberUser;
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
