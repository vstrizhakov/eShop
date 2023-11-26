namespace eShop.Messaging.Models.Identity
{
    public class GetIdentityUserResponse : Messaging.Message, IResponse
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
