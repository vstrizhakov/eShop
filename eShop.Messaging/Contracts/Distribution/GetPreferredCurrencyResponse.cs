using eShop.Messaging.Contracts;

namespace eShop.Messaging.Contracts.Distribution
{
    public class GetPreferredCurrencyResponse
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
