using eShop.Distribution.Entities;

namespace eShop.Distribution.Aggregates
{
    public class ShopFilter
    {
        public Shop Shop { get; }
        public bool IsEnabled { get; }

        public ShopFilter(Shop shop, bool isEnabled)
        {
            Shop = shop;
            IsEnabled = isEnabled;
        }
    }
}
