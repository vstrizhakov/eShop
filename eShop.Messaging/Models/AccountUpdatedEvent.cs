namespace eShop.Messaging.Models
{
    public class AccountUpdatedEvent : Messaging.Message
    {
        public Account Account { get; set; }
    }
}
