using eShopping.Messaging.Contracts;

namespace eShopping.Messaging.Contracts.Distribution
{
    public class SetCurrencyRateResponse
    {
        public Guid AccountId { get; }
        public Currency PreferredCurrency { get; }
        public IEnumerable<CurrencyRate> CurrencyRates { get; }

        public SetCurrencyRateResponse(Guid accountId, Currency preferredCurrency, IEnumerable<CurrencyRate> currencyRates)
        {
            AccountId = accountId;
            PreferredCurrency = preferredCurrency;
            CurrencyRates = currencyRates;
        }
    }

}
