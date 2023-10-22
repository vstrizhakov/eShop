namespace eShop.Messaging.Models.Identity
{
    public class RegisterIdentityUserResponse : Messaging.Message, IResponse
    {
        public string IdentityUserId { get; set; }
        public Guid AccountId { get; set; }
    }
}
