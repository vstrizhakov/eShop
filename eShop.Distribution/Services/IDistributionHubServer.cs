using eShop.Distribution.Entities;

namespace eShop.Distribution.Services
{
    public interface IDistributionHubServer
    {
        Task SendRequestUpdatedAsync(DistributionGroupItem request);
    }
}
