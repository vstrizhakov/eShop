namespace eShop.Messaging.Models.Distribution
{
    public class GetCurrencyRatesRequest : Messaging.Message, IRequest<GetCurrencyRatesResponse>
    {
        public Guid AccountId { get; }

        public GetCurrencyRatesRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }

}
