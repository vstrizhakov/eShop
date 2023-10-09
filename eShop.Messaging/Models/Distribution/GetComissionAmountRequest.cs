namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionAmountRequest : Messaging.Message, IRequest<GetComissionAmountResponse>
    {
        public Guid AccountId { get; }

        public GetComissionAmountRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
