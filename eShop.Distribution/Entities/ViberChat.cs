namespace eShop.Distribution.Entities
{
    public class ViberChat
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public bool IsEnabled { get; set; }

        public Account Account { get; set; }
    }
}
