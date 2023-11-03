namespace eShop.Messaging.Models
{
    public class Announce
    {
        public Guid Id { get; set; }
        public Guid ShopId { get; set; }
        public IEnumerable<Uri> Images { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
