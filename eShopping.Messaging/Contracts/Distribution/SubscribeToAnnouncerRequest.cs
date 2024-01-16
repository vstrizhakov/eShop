namespace eShopping.Messaging.Contracts.Distribution
{
    public class SubscribeToAnnouncerRequest
    {
        public Guid? AccountId { get; set; }
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public Guid AnnouncerId { get; set; }
    }
}
