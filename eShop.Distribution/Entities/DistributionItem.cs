using eShop.Distribution.Entities.History;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(DistributionId), nameof(Id))]
    public class DistributionItem
    {
        public Guid DistributionId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DistributionItemStatus Status { get; set; }
        public Guid? TelegramChatId { get; set; }
        public Guid? ViberChatId { get; set; }
        public Guid DistributionSettingsId { get; set; }
        public Guid AccountId { get; set; }

        public Distribution Distribution { get; set; }
        public TelegramChat? TelegramChat { get; set; }
        public ViberChat? ViberChat { get; set; }
        public DistributionSettingsRecord DistributionSettings { get; set; }
        public Account Account { get; set; }
    }
}
