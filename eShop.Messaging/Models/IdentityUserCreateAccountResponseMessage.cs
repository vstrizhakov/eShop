namespace eShop.Messaging.Models
{
    public class IdentityUserCreateAccountResponseMessage
    {
        public string IdentityUserId { get; set; }
        public Guid AccountId { get; set; }
    }
}
