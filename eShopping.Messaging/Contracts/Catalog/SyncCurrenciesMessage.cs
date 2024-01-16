using eShopping.Messaging.Contracts;

namespace eShopping.Messaging.Contracts.Catalog
{
    public class SyncCurrenciesMessage
    {
        public IEnumerable<Currency> Currencies { get; set; }
    }
}
