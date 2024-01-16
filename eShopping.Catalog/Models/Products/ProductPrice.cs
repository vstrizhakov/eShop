using eShopping.Catalog.Models.Currencies;

namespace eShopping.Catalog.Models.Products
{
    public class ProductPrice
    {
        public Currency Currency { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}
