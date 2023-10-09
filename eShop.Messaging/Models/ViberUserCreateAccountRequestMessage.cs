namespace eShop.Messaging.Models
{
    public class ViberUserCreateAccountRequestMessage : Messaging.Message
    {
        public Guid ViberUserId { get; set; }
        public Guid ProviderId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
