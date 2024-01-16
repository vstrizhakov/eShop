using eShopping.Distribution.Entities.History;

namespace eShopping.Distribution.Entities
{
    public class DistributionItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DistributionItemStatus Status { get; set; }
        public Guid? TelegramChatId { get; set; }
        public Guid? ViberChatId { get; set; }

        public TelegramChat? TelegramChat { get; set; }
        public ViberChat? ViberChat { get; set; }
    }
}
