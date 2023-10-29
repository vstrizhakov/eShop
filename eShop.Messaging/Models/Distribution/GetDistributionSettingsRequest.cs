namespace eShop.Messaging.Models.Distribution
{
    public sealed class GetDistributionSettingsRequest : Messaging.Message, IRequest<GetDistributionSettingsResponse>
    {
        public Guid AccountId { get; set; }

        public GetDistributionSettingsRequest(Guid accountId)
        {
            AccountId = accountId;
        }
    }
}
