namespace eShop.Messaging.Models.Catalog
{
    public class GetCurrenciesRequest : Messaging.Message, IRequest<GetCurrenciesResponse>
    {
        public Guid AccountId { get; }

        public GetCurrenciesRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
