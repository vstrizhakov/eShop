namespace eShop.Messaging.Models.Distribution
{
    public class SetComissionShowRequest : Messaging.Message, IRequest<SetComissionShowResponse>
    {
        public Guid AccountId { get; }
        public bool Show { get; }

        public SetComissionShowRequest(Guid accountId, bool show)
        {
            AccountId = accountId;
            Show = show;
        }
    }

}
