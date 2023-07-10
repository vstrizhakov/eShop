namespace eShop.Catalog.Entities
{
    public class CompositionImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Path { get; set; }
        public Guid CompositionId { get; set; }

        public Composition Composition { get; set; }
    }
}
