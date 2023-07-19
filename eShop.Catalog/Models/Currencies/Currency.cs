namespace eShop.Catalog.Models.Currencies
{
    public class Currency
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
