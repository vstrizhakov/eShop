using System.Runtime.Serialization;

namespace eShop.Distribution.Models
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