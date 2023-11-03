namespace eShop.Catalog.Models.Products
{
    public class CreateProductPrice
    {
        public Guid? CurrencyId { get; set; }
        public double Price { get; set; }
        public double? DiscountedPrice { get; set; }
    }
}
