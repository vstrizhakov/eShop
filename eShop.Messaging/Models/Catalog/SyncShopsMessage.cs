namespace eShop.Messaging.Models.Catalog
{
    public class SyncShopsMessage : Messaging.Message
    {
        public IEnumerable<Shop> Shops { get; set; }
    }
}
