using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace eShop.ViberBot
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MessageType
    {
        [EnumMember(Value = "text")]
        Text,
        [EnumMember(Value = "picture")]
        Picture,
        [EnumMember(Value = "video")]
        Video,
        [EnumMember(Value = "file")]
        File,
        [EnumMember(Value = "location")]
        Location,
        [EnumMember(Value = "contact")]
        Contact,
        [EnumMember(Value = "sticker")]
        Sticker,
        [EnumMember(Value = "rich_media")]
        CarouselContent,
        [EnumMember(Value = "url")]
        Url,
    }
}