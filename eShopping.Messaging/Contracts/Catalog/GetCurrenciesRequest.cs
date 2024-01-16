namespace eShopping.Messaging.Contracts.Catalog
{
    public class GetCurrenciesRequest
    {
        public Guid AccountId { get; }

        public GetCurrenciesRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
