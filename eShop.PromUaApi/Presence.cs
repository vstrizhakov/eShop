using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShop.PromUaApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Presence
    {
        [EnumMember(Value = "available")]
        Available,
        [EnumMember(Value = "not_available")]
        NotAvailable,
        [EnumMember(Value = "order")]
        Order,
        [EnumMember(Value = "service")]
        Service,
    }
}