namespace eShop.Messaging.Models.Distribution
{
    public class GetCurrencyRatesResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public Currency PreferredCurrency { get; }
        public IEnumerable<CurrencyRate> CurrencyRates { get; }

        public GetCurrencyRatesResponse(Guid accountId, Currency preferredCurrency, IEnumerable<CurrencyRate> currencyRates)
        {
            AccountId = accountId;
            PreferredCurrency = preferredCurrency;
            CurrencyRates = currencyRates;
        }
    }

}
