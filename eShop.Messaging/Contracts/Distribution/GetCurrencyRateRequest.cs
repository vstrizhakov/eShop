namespace eShop.Messaging.Contracts.Distribution
{
    public class GetCurrencyRateRequest
    {
        public Guid AccountId { get; }
        public Guid CurrencyId { get; }

        public GetCurrencyRateRequest(Guid accountId, Guid currencyId)
        {
            AccountId = accountId;
            CurrencyId = currencyId;
        }
    }

}
