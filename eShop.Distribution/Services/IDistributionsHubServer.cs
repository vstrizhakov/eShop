using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionsHubServer
    {
        Task SendRequestUpdatedAsync(DistributionItem request);
    }
}
