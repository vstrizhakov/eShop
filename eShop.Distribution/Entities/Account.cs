namespace eShop.Distribution.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActivated { get; set; }
        public Guid ProviderId { get; set; }

        public DistributionSettings ActiveDistributionSettings => DistributionSettings.OrderByDescending(e => e.CreatedAt).FirstOrDefault()!;

        public ICollection<TelegramChat> TelegramChats { get; set; } = new List<TelegramChat>();
        public ViberChat? ViberChat { get; set; }
        public ICollection<DistributionSettings> DistributionSettings { get; set; } = new List<DistributionSettings>();
    }
}
