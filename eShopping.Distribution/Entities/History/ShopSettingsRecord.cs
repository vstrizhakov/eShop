using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Entities.History
{
    public class ShopSettingsRecord
    {
        public bool Filter { get; set; }
        public ICollection<EmbeddedShop> PreferredShops { get; set; } = new List<EmbeddedShop>();
    }
}
