namespace eShop.Messaging.Contracts.Distribution
{
    public record SubscribeToAnnouncerResponse
    {
        public Guid? AccountId { get; set; }
        public Guid? TelegramUserId { get; set; }
        public Guid? ViberUserId { get; set; }
        public bool Succeeded { get; set; }
        public Announcer Announcer { get; set; }
    }
}
