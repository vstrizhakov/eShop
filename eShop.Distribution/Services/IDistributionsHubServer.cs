using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionsHubServer
    {
        Task SendRequestUpdatedAsync(Guid distributionId, DistributionItem request);
    }
}
