namespace eShop.Messaging.Models.Identity
{
    public class GetIdentityUserRequest : Messaging.Message, IRequest<GetIdentityUserResponse>
    {
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
