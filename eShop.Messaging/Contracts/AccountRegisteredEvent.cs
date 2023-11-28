namespace eShop.Messaging.Contracts
{
    public class AccountRegisteredEvent
    {
        public Account Account { get; set; }
        public Guid? AnnouncerId { get; set; }
    }
}
