using eShop.Distribution.Entities;
using eShop.Messaging.Models;

namespace eShop.Distribution.Services
{
    public interface IDistributionService
    {
        Task<Entities.Distribution> CreateDistributionAsync(Guid announcerId, Announce composition);
        Task<Entities.Distribution?> GetDistributionAsync(Guid distributionId);
        Task UpdateDistributionRequestStatusAsync(Guid distributionRequestId, bool deliveryFailed);
    }
}
