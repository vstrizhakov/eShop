namespace eShop.Messaging.Models.Distribution
{
    public class GetPreferredCurrencyRequest : Messaging.Message, IRequest<GetPreferredCurrencyResponse>
    {
        public Guid AccountId { get; }

        public GetPreferredCurrencyRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
