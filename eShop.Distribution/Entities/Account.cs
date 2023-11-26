namespace eShop.Distribution.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActivated { get; set; }
        public Guid? AnnouncerId { get; set; }

        public ICollection<TelegramChat> TelegramChats { get; set; } = new List<TelegramChat>();
        public ViberChat? ViberChat { get; set; }
        public DistributionSettings DistributionSettings { get; set; }
    }
}
