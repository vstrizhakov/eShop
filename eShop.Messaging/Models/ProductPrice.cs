namespace eShop.Messaging.Models
{
    public class ProductPrice
    {
        public Currency Currency { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}
