namespace eShop.Messaging.Models.Catalog
{
    public class GetCurrenciesResponse : Messaging.Message, IResponse
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
