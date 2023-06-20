using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShop.PromUaApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SellingType
    {
        [EnumMember(Value = "retail")]
        Retail,
        [EnumMember(Value = "wholesale")]
        Wholesale,
        [EnumMember(Value = "universal")]
        Universal,
        [EnumMember(Value = "service")]
        Service,
    }
}