namespace eShop.Messaging.Models
{
    public class AccountRegisteredEvent : Messaging.Message
    {
        public Account Account { get; set; }
        public Guid? AnnouncerId { get; set; }
    }
}
