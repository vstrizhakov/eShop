namespace eShop.Catalog.Entities
{
    public class AnnounceImage
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
        public Guid AnnounceId { get; set; }

        public Announce Announce { get; set; }
    }
}
