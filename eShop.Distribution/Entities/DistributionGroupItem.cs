using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(GroupId), nameof(Id))]
    public class DistributionGroupItem
    {
        public Guid GroupId { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public DistributionGroupItemStatus Status { get; set; }
        public Guid? TelegramChatId { get; set; }
        public Guid? ViberChatId { get; set; }
        public Guid DistributionSettingsId { get; set; }

        public DistributionGroup Group { get; set; }
        public TelegramChat? TelegramChat { get; set; }
        public ViberChat? ViberChat { get; set; }
        public DistributionSettingsHistoryRecord DistributionSettings { get; set; }
    }
}
