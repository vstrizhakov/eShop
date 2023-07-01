namespace eShop.Distribution.Entities
{
    public class Account
    {
        public Guid Id { get; set; }

        public Guid ProviderId { get; set; }

        public ICollection<TelegramChat> TelegramChats { get; set; } = new List<TelegramChat>();
        public ViberChat? ViberChat { get; set; }
    }
}
