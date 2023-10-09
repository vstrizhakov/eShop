namespace eShop.Messaging.Models.Catalog
{
    public class SyncShopsMessage
    {
        public IEnumerable<Shop> Shops { get; set; }
    }
}
