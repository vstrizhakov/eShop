using System.Runtime.Serialization;

namespace eShopping.Distribution.Models.Distributions
{
    public enum DeliveryStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "filtered")]
        Filtered,
    }
}