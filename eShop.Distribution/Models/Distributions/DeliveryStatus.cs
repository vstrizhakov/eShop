using System.Runtime.Serialization;

namespace eShop.Distribution.Models.Distributions
{
    public enum DeliveryStatus
    {
        [EnumMember(Value = "pending")]
        Pending,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "failed")]
        Failed,
    }
}