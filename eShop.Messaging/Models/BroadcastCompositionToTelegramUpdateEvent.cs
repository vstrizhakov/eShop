namespace eShop.Messaging.Models
{
    public class BroadcastCompositionToTelegramUpdateEvent
    {
        public Guid DistributionGroupId { get; set; }
        public Guid TelegramChatId { get; set; }
        public bool Succeeded { get; set; }
    }
}
