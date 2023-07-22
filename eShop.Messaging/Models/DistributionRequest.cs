namespace eShop.Messaging.Models
{
    public class DistributionRequest
    {
        public Guid RequestId { get; set; }
        public Guid TargetId { get; set; }
    }
}
