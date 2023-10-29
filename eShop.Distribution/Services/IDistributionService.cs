using eShop.Distribution.Entities;
using eShop.Messaging.Models;

namespace eShop.Distribution.Services
{
    public interface IDistributionService
    {
        Task<DistributionGroup> CreateDistributionAsync(Guid providerId, Composition composition);
        Task UpdateDistributionRequestStatusAsync(Guid distributionRequestId, bool deliveryFailed);
    }
}
