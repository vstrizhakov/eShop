namespace eShop.Messaging.Contracts.Viber
{
    public class RegisterViberUserResponse
    {
        public bool IsSuccess { get; set; }
        public Guid ViberUserId { get; set; }
        public Guid? AccountId { get; set; }
        public string? ProviderEmail { get; set; }
        public bool IsConfirmationRequested { get; set; }
    }
}
