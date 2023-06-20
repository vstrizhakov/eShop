namespace eShop.Database.Data
{
    public class CompositionImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Path { get; set; }
        public string CompositionId { get; set; }

        public Composition Composition { get; set; }
    }
}
