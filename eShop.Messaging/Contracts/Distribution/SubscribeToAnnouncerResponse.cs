using MassTransit;

namespace eShop.Messaging.Contracts.Distribution
{
    public record SubscribeToAnnouncerResponse : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public Guid AccountId { get; set; }
        public bool Succeeded { get; set; }
        public Announcer Announcer { get; set; }
    }
}
