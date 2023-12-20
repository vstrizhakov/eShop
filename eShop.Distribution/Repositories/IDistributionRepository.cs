namespace eShop.Distribution.Repositories
{
    public interface IDistributionRepository
    {
        Task CreateDistributionAsync(Entities.Distribution distribution);
        Task<Entities.Distribution?> GetDistributionAsync(Guid id, Guid announcerId);
        Task UpdateDistributionAsync(Entities.Distribution item);
    }
}
