using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShop.ViberBot
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        [EnumMember(Value = "webhook")]
        Webhook,
        [EnumMember(Value = "delivered")]
        Delivered,
        [EnumMember(Value = "seen")]
        Seen,
        [EnumMember(Value = "failed")]
        Failed,
        [EnumMember(Value = "subscribed")]
        Subscribed,
        [EnumMember(Value = "unsubscribed")]
        Unsubscribed,
        [EnumMember(Value = "conversation_started")]
        ConversationStarted,
        [EnumMember(Value = "message")]
        Message,
        [EnumMember(Value = "client_status")]
        ClientStatus,
        [EnumMember(Value = "action")]
        Action,
    }
}
