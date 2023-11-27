namespace eShop.Messaging.Contracts
{
    public class Product
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Uri Url { get; set; }
        public ProductPrice Price { get; set; }
    }
}