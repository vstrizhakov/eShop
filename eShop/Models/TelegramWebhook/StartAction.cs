using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShop.Models.TelegramWebhook
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StartAction
    {
        [EnumMember(Value = "rc")]
        RegisterClient,
    }
}
