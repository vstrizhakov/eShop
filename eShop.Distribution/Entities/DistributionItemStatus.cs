using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace eShop.Distribution.Entities
{
    [JsonConverter(typeof(StringEnumConverter), typeof(CamelCaseNamingStrategy))]
    public enum DistributionItemStatus
    {
        Pending,
        Delivered,
        Failed,
        Filtered,
    }
}
