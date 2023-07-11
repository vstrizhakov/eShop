using eShop.Distribution.Entities;

namespace eShop.Distribution.Models
{
    public class DistributionItem
    {
        public Guid Id { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public Guid? ViberChatId { get; set; }
        public Guid? TelegramChatId { get; set; }
    }
}