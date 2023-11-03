using eShop.Distribution.Entities;

namespace eShop.Distribution.Repositories
{
    public interface IDistributionRepository
    {
        Task CreateDistributionAsync(Entities.Distribution distribution);
        Task<Entities.Distribution?> GetDistributionByIdAsync(Guid id);
        Task<DistributionItem?> GetDistributionRequestAsync(Guid distributionRequestId);
        Task UpdateDistributionItemAsync(DistributionItem item);
    }
}
