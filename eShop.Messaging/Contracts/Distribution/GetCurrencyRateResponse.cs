namespace eShop.Messaging.Contracts.Distribution
{
    public record GetCurrencyRateResponse
    {
        public Guid AccountId { get; init; }
        public bool Succeeded { get; init; }
        public Currency? PreferredCurrency { get; init; }
        public CurrencyRate? CurrencyRate { get; init; }
    }

}
