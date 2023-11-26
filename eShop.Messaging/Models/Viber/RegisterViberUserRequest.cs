namespace eShop.Messaging.Models.Viber
{
    public class RegisterViberUserRequest : Messaging.Message, IRequest<RegisterViberUserResponse>
    {
        public Guid ViberUserId { get; set; }
        public Guid? AnnouncerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
