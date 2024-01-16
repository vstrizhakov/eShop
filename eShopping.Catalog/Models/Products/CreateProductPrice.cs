namespace eShopping.Catalog.Models.Products
{
    public class CreateProductPrice
    {
        public Guid CurrencyId { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}
