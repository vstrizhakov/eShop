using eShop.Messaging.Contracts.Distribution;

namespace eShop.Messaging.Contracts.Viber
{
    public class RegisterViberUserResponse
    {
        public bool IsSuccess { get; set; }
        public Guid ViberUserId { get; set; }
        public Guid? AccountId { get; set; }
        public Announcer? Announcer { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
