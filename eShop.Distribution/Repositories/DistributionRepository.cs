using eShop.Distribution.DbContexts;
using eShop.Distribution.Entities;
using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Repositories
{
    public class DistributionRepository : IDistributionRepository
    {
        private readonly DistributionDbContext _context;

        public DistributionRepository(DistributionDbContext context)
        {
            _context = context;
        }

        public async Task CreateDistributionGroupAsync(DistributionGroup distributionGroup)
        {
            _context.DistributionGroups.Add(distributionGroup);

            await _context.SaveChangesAsync();
        }

        public async Task<DistributionGroup?> GetDistributionGroupByIdAsync(Guid id)
        {
            var distributionGroup = await _context.DistributionGroups
                .Include(e => e.Items)
                .FirstOrDefaultAsync(e => e.Id == id);
            return distributionGroup;
        }

        public async Task<DistributionGroupItem?> GetDistributionGroupItemByTelegramChatIdAsync(Guid distributionGroupId, Guid telegramChatId)
        {
            var distributionGroupItem = await _context.DistributionGroupItems
                .FirstOrDefaultAsync(e => e.GroupId == distributionGroupId && e.TelegramChatId == telegramChatId);
            return distributionGroupItem;
        }

        public async Task<DistributionGroupItem?> GetDistributionGroupItemByViberChatIdAsync(Guid distributionGroupId, Guid viberChatId)
        {
            var distributionGroupItem = await _context.DistributionGroupItems
                .FirstOrDefaultAsync(e => e.GroupId == distributionGroupId && e.ViberChatId == viberChatId);
            return distributionGroupItem;
        }

        public async Task UpdateDistributionGroupItemAsync(DistributionGroupItem distributionItem)
        {
            await _context.SaveChangesAsync();
        }
    }
}
