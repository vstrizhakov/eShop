using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDistributionRepository
    {
        Task CreateDistributionGroupAsync(DistributionGroup distributionGroup);
        Task<DistributionGroup?> GetDistributionGroupByIdAsync(Guid id);
        Task<DistributionGroupItem?> GetDistributionRequestAsync(Guid distributionRequestId);
        Task UpdateDistributionGroupItemAsync(DistributionGroupItem distributionItem);
    }
}
