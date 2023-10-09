namespace eShop.Messaging.Models
{
    public class IdentityUserCreateAccountResponseMessage : Messaging.Message
    {
        public string IdentityUserId { get; set; }
        public Guid AccountId { get; set; }
    }
}
