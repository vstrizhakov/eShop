namespace eShop.Distribution.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public Chat ViberChat { get; set; }
        public IEnumerable<Chat> TelegramChats { get; set; }
    }
}
