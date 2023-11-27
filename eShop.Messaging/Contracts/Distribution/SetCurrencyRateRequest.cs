namespace eShop.Messaging.Contracts.Distribution
{
    public class SetCurrencyRateRequest
    {
        public Guid AccountId { get; }
        public Guid CurrencyId { get; }
        public double Rate { get; }

        public SetCurrencyRateRequest(Guid accountId, Guid currencyId, double rate)
        {
            AccountId = accountId;
            CurrencyId = currencyId;
            Rate = rate;
        }
    }

}
