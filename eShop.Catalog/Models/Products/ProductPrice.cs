using eShop.Catalog.Models.Currencies;

namespace eShop.Catalog.Models.Products
{
    public class ProductPrice
    {
        public Currency Currency { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}
