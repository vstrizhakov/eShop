using eShop.Messaging.Contracts;

namespace eShop.Distribution.Services
{
    public interface IDistributionService
    {
        Task<Entities.Distribution> CreateDistributionAsync(Guid announcerId, Announce composition);
        Task<Entities.Distribution?> GetDistributionAsync(Guid distributionId, Guid announcerId);
        Task SetDistributionItemStatusAsync(Guid distributionId, Guid announcerId, Guid distributionItemId, bool deliveryFailed);
    }
}
