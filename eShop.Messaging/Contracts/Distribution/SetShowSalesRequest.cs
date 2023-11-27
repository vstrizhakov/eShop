namespace eShop.Messaging.Contracts.Distribution
{
    public sealed class SetShowSalesRequest
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
