namespace eShopping.Catalog.Entities
{
    public class AnnounceImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; }
    }
}
