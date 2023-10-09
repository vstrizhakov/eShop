namespace eShop.Messaging.Models.Distribution
{
    public class GetComissionSettingsResponse : Messaging.Message, IResponse
    {
        public Guid AccountId { get; }
        public bool Show { get; }
        public decimal Amount { get; }

        public GetComissionSettingsResponse(Guid accountId, bool show, decimal amount)
        {
            AccountId = accountId;
            Show = show;
            Amount = amount;
        }
    }

}
