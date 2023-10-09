namespace eShop.Messaging.Models.Catalog
{
    public class SyncCurrenciesMessage : Messaging.Message
    {
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
