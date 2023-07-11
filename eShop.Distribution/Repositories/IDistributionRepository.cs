using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDistributionRepository
    {
        Task CreateDistributionGroupAsync(DistributionGroup distributionGroup);
        Task<DistributionGroup?> GetDistributionGroupByIdAsync(Guid id);
        Task<DistributionGroupItem?> GetDistributionGroupItemByTelegramChatIdAsync(Guid distributionGroupId, Guid telegramChatId);
        Task<DistributionGroupItem?> GetDistributionGroupItemByViberChatIdAsync(Guid distributionGroupId, Guid viberChatId);
        Task UpdateDistributionGroupItemAsync(DistributionGroupItem distributionItem);
    }
}
