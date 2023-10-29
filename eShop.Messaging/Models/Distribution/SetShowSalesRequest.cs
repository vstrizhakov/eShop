namespace eShop.Messaging.Models.Distribution
{
    public sealed class SetShowSalesRequest : Messaging.Message, IRequest<SetShowSalesResponse>
    {
        public Guid AccountId { get; set; }
        public bool ShowSales { get; set; }

        public SetShowSalesRequest(Guid accountId, bool showSales)
        {
            AccountId = accountId;
            ShowSales = showSales;
        }
    }
}
