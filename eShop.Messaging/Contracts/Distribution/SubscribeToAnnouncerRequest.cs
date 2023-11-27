using MassTransit;

namespace eShop.Messaging.Contracts.Distribution
{
    public class SubscribeToAnnouncerRequest : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; } = NewId.NextGuid();
        public Guid AccountId { get; set; }
        public Guid AnnouncerId { get; set; }
    }
}
