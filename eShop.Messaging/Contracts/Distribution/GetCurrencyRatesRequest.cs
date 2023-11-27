namespace eShop.Messaging.Contracts.Distribution
{
    public class GetCurrencyRatesRequest
    {
        public Guid AccountId { get; }

        public GetCurrencyRatesRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
