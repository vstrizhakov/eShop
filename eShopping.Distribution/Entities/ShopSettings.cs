namespace eShopping.Distribution.Entities
{
    public class ShopSettings
    {
        public bool Filter { get; set; }
        public ICollection<EmbeddedShop> PreferredShops { get; set; } = new List<EmbeddedShop>();
    }
}
