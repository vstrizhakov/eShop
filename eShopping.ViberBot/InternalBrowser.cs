using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShopping.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class InternalBrowser
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ActionButton { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ActionReplyData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Mode { get; set; }
    }
}
