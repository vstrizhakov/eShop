using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface IShopService
    {
        Task<IEnumerable<Shop>> GetShopsAsync();
    }
}
