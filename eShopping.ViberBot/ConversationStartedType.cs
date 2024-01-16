using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShopping.ViberBot
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ConversationStartedType
    {
        [EnumMember(Value = "open")]
        Open,
    }
}