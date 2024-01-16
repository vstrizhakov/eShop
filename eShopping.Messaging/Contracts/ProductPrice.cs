namespace eShopping.Messaging.Contracts
{
    public class ProductPrice
    {
        public Currency Currency { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}
