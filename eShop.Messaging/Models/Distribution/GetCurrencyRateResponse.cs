namespace eShop.Messaging.Models.Distribution
{
    public class GetCurrencyRateResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public bool IsSucceeded { get; }
        public Currency? PreferredCurrency { get; }
        public CurrencyRate? CurrencyRate { get; }

        public GetCurrencyRateResponse(Guid accountId, bool isSucceeded, Currency? preferredCurrency, CurrencyRate? currencyRate)
        {
            AccountId = accountId;
            IsSucceeded = isSucceeded;
            PreferredCurrency = preferredCurrency;
            CurrencyRate = currencyRate;
        }
    }

}
