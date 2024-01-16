
namespace eShopping.Distribution.Services
{
    public interface IDistributionService
    {
        Task<Entities.Distribution> CreateDistributionAsync(Guid announcerId, Messaging.Contracts.Announce composition);
        Task<Entities.Distribution?> GetDistributionAsync(Guid distributionId, Guid announcerId);
        Task SetDistributionItemStatusAsync(Guid distributionId, Guid announcerId, Guid distributionItemId, bool deliveryFailed);
    }
}
