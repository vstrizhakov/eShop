using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShopping.PromUaApi
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DiscountType
    {
        [EnumMember(Value = "amount")]
        Amount,
        [EnumMember(Value = "percent")]
        Percent,
    }
}