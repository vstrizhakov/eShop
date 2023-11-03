using eShop.Catalog.Models.Announces;

namespace eShop.Catalog.Hubs
{
    public interface IAnnouncesHubClient
    {
        Task AnnounceUpdated(Announce announce);
    }
}
