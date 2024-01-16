using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Services
{
    public interface IDistributionsHubServer
    {
        Task SendRequestUpdatedAsync(Guid distributionId, DistributionItem request);
    }
}
