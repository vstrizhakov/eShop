using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(DistributionSettingsId), nameof(CurrencyId))]
    public class CurrencyRateHistoryRecord
    {
        public Guid CurrencyId { get; set; }
        public Guid DistributionSettingsId { get; set; }
        public double Rate { get; set; }

        public Currency Currency { get; set; }
        public DistributionSettingsHistoryRecord DistributionSettings { get; set; }
    }
}
