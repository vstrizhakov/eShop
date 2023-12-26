namespace eShop.Messaging.Contracts.Distribution
{
    public class SetCurrencyRateRequest
    {
        public Guid AccountId { get; }
        public Guid CurrencyId { get; }
        public float Rate { get; }

        public SetCurrencyRateRequest(Guid accountId, Guid currencyId, float rate)
        {
            AccountId = accountId;
            CurrencyId = currencyId;
            Rate = rate;
        }
    }

}
