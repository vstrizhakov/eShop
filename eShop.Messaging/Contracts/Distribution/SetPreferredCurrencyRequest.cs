namespace eShop.Messaging.Contracts.Distribution
{
    public class SetPreferredCurrencyRequest
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
