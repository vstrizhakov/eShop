namespace eShop.Messaging.Models.Identity
{
    public class RegisterIdentityUserRequest : Messaging.Message, IRequest<RegisterIdentityUserResponse>
    {
        public string IdentityUserId { get; set; }
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
