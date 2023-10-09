namespace eShop.Messaging.Models.Distribution
{
    public class SetPreferredCurrencyRequest : Messaging.Message, IRequest<SetPreferredCurrencyResponse>
    {
        public Guid AccountId { get; }
        public Guid CurrencyId { get; }

        public SetPreferredCurrencyRequest(Guid accountId, Guid currencyId)
        {
            AccountId = accountId;
            CurrencyId = currencyId;
        }
    }

}
