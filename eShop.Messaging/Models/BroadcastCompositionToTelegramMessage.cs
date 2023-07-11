namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToTelegramMessage
    {
        public Guid DistributionGroupId { get; set; }
        public IEnumerable<Guid> TelegramChatIds { get; set; }
        public Uri Image { get; set; }
        public string Caption { get; set; }
    }
}
