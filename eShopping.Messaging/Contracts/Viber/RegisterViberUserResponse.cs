using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Messaging.Contracts.Viber
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
