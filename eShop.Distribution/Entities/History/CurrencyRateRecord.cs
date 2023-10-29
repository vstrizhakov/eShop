using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities.History
{
    [PrimaryKey(nameof(DistributionSettingsId), nameof(CurrencyId))]
    public class CurrencyRateRecord
    {
        public Guid CurrencyId { get; set; }
        public Guid DistributionSettingsId { get; set; }
        public double Rate { get; set; }

        public Currency Currency { get; set; }
        public DistributionSettingsRecord DistributionSettings { get; set; }
    }
}
