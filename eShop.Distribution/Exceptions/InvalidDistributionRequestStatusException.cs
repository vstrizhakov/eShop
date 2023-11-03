using eShop.Distribution.Entities;

namespace eShop.Distribution.Exceptions
{
    public class InvalidDistributionRequestStatusException : Exception
    {
        public InvalidDistributionRequestStatusException() : base($"Requested distribution request has status different from {nameof(DistributionItemStatus.Pending)}")
        {
        }
    }
}
