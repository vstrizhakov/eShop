using eShopping.Distribution.Models.Distributions;

namespace eShopping.Distribution.Hubs
{
    public interface IDistributionsHubClient
    {
        Task RequestUpdated(DistributionItem distributionItem);
    }
}
