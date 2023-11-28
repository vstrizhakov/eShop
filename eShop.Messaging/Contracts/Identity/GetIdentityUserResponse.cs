namespace eShop.Messaging.Contracts.Identity
{
    public class GetIdentityUserResponse
    {
        public string? IdentityUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? AnnouncerId { get; set; }
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
