namespace eShop.Messaging.Models.Identity
{
    public class RegisterIdentityUserRequest : Messaging.Message, IRequest<RegisterIdentityUserResponse>
    {
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
