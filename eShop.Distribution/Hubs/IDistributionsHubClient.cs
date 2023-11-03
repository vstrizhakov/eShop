using eShop.Distribution.Models.Distributions;

namespace eShop.Distribution.Hubs
{
    public interface IDistributionsHubClient
    {
        Task RequestUpdated(DistributionItem distributionItem);
    }
}
