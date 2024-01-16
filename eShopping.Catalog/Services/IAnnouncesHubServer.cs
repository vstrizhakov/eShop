using eShopping.Catalog.Entities;

namespace eShopping.Catalog.Services
{
    public interface IAnnouncesHubServer
    {
        Task SendAnnounceUpdatedAsync(Announce announce);
    }
}
