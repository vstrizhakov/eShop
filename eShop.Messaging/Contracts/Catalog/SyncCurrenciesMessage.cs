namespace eShop.Messaging.Contracts.Catalog
{
    public class SyncCurrenciesMessage
    {
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
