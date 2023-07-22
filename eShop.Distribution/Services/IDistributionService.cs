using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionService
    {
        Task<DistributionGroup> CreateDistributionFromProviderIdAsync(Guid providerId);
        Task UpdateDistributionRequestStatusAsync(Guid distributionRequestId, bool deliveryFailed);
    }
}
