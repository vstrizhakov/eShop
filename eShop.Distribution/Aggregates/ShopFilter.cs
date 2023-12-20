using eShop.Distribution.Entities;

namespace eShop.Distribution.Aggregates
{
    public class ShopFilter
    {
        public EmbeddedShop Shop { get; }
        public bool IsEnabled { get; }

        public ShopFilter(EmbeddedShop shop, bool isEnabled)
        {
            Shop = shop;
            IsEnabled = isEnabled;
        }
    }
}
