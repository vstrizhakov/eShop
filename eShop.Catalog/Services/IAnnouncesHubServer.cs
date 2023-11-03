using eShop.Catalog.Entities;

namespace eShop.Catalog.Services
{
    public interface IAnnouncesHubServer
    {
        Task SendAnnounceUpdatedAsync(Announce announce);
    }
}
