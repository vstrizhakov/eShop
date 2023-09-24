namespace eShop.Messaging.Models
{
    public class SyncCurrenciesMessage
    {
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
