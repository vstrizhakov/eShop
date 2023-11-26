namespace eShop.Messaging.Models.Distribution
{
    public class SubscribeToAnnouncerResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; set; }
        public bool Succeeded { get; set; }
        public Announcer Announcer { get; set; }
    }
}
