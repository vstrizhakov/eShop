namespace eShop.Messaging.Models.Viber
{
    public class RegisterViberUserResponse : Messaging.Message, IResponse
    {
        public bool IsSuccess { get; set; }
        public Guid ViberUserId { get; set; }
        public Guid? AccountId { get; set; }
        public string? ProviderEmail { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
