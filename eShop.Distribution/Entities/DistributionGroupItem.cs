using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(GroupId), nameof(Id))]
    public class DistributionGroupItem
    {
        public Guid GroupId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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
