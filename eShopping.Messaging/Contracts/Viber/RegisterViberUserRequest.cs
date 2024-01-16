namespace eShopping.Messaging.Contracts.Viber
{
    public class RegisterViberUserRequest
    {
        public Guid ViberUserId { get; set; }
        public Guid? AnnouncerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
