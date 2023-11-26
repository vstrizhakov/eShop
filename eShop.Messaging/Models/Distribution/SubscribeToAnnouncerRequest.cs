namespace eShop.Messaging.Models.Distribution
{
    public class SubscribeToAnnouncerRequest : Messaging.Message, IRequest<SubscribeToAnnouncerResponse>
    {
        public Guid AccountId { get; set; }
        public Guid AnnouncerId { get; set; }
    }
}
