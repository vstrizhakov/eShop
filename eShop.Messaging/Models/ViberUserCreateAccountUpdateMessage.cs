namespace eShop.Messaging.Models
{
    public class ViberUserCreateAccountUpdateMessage : Messaging.Message
    {
        public bool IsSuccess { get; set; }
        public Guid? AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? ViberUserId { get; set; }
        public Guid? ProviderId { get; set; }
        public string? ProviderEmail { get; set; }
    }
}
