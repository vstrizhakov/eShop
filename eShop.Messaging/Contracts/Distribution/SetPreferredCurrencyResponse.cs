using eShop.Messaging.Contracts;

namespace eShop.Messaging.Contracts.Distribution
{
    public class SetPreferredCurrencyResponse
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
