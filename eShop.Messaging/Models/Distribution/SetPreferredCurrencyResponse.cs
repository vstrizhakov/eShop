namespace eShop.Messaging.Models.Distribution
{
    public class SetPreferredCurrencyResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public bool Succeeded { get; }
        public Currency? PreferredCurrency { get; }

        public SetPreferredCurrencyResponse(Guid accountId, bool succeeded, Currency? preferredCurrency)
        {
            AccountId = accountId;
            Succeeded = succeeded;
            PreferredCurrency = preferredCurrency;
        }
    }

}
