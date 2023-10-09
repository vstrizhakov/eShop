namespace eShop.Messaging.Models.Catalog
{
    public class SyncCurrenciesMessage
    {
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
