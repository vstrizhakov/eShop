namespace eShop.Distribution.Entities
{
    public class Shop
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
    }
}
