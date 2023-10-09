namespace eShop.Messaging.Models.Distribution
{
    public class GetPreferredCurrencyResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public Currency? PreferredCurrency { get; }

        public GetPreferredCurrencyResponse(Guid accountId, Currency? preferredCurrency)
        {
            AccountId = accountId;
            PreferredCurrency = preferredCurrency;
        }
    }

}
