namespace eShop.Messaging.Contracts.Distribution
{
    public class GetCurrencyRatesResponse
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
