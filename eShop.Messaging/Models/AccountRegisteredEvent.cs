namespace eShop.Messaging.Models
{
    public class AccountRegisteredEvent : Messaging.Message
    {
        public Account Account { get; set; }
        public Guid? ProviderId { get; set; }
    }
}
