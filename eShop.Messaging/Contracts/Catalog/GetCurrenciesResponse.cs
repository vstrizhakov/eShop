namespace eShop.Messaging.Contracts.Catalog
{
    public class GetCurrenciesResponse
    {
        public Guid AccountId { get; }
        public IEnumerable<Currency> Currencies { get; }

        public GetCurrenciesResponse(Guid accountId, IEnumerable<Currency> currencies)
        {
            AccountId = accountId;
            Currencies = currencies;
        }
    }
}
