namespace eShopping.Messaging.Contracts.Distribution
{
    public class SetCurrencyRateRequest
    {
        public Guid AccountId { get; }
        public Guid CurrencyId { get; }
        public decimal Rate { get; }

        public SetCurrencyRateRequest(Guid accountId, Guid currencyId, decimal rate)
        {
            AccountId = accountId;
            CurrencyId = currencyId;
            Rate = rate;
        }
    }

}
