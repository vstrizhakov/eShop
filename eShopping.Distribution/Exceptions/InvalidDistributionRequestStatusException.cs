using eShopping.Distribution.Entities;

namespace eShopping.Distribution.Exceptions
{
    public class InvalidDistributionRequestStatusException : Exception
    {
        public InvalidDistributionRequestStatusException() : base($"Requested distribution request has status different from {nameof(DistributionItemStatus.Pending)}")
        {
        }
    }
}
