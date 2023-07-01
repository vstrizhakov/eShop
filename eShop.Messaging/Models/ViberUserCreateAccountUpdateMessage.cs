namespace eShop.Messaging.Models
{
    public class ViberUserCreateAccountUpdateMessage
    {
        public bool IsSuccess { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? ViberUserId { get; set; }
        public Guid? ProviderId { get; set; }
        public string? ProviderEmail { get; set; }
    }
}
