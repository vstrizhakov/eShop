namespace eShopping.Messaging.Contracts.Distribution
{
    public class GetPreferredCurrencyRequest
    {
        public Guid AccountId { get; }

        public GetPreferredCurrencyRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
