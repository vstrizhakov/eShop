using eShop.Distribution.Models;

namespace eShop.Distribution.Hubs
{
    public interface IDistributionClient
    {
        Task RequestUpdated(DistributionItem distributionItem);
    }
}
