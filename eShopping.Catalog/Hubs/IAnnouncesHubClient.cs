using eShopping.Catalog.Models.Announces;

namespace eShopping.Catalog.Hubs
{
    public interface IAnnouncesHubClient
    {
        Task AnnounceUpdated(Announce announce);
    }
}
