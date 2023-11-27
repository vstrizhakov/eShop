namespace eShop.Messaging.Contracts
{
    public class AccountUpdatedEvent
    {
        public Account Account { get; set; }
        public Guid? AnnouncerId { get; set; }
    }
}
