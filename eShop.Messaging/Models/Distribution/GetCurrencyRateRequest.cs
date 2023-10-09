namespace eShop.Messaging.Models.Distribution
{
    public class GetCurrencyRateRequest : Messaging.Message, IRequest<GetCurrencyRateResponse>
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
