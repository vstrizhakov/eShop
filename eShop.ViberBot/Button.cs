using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShop.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class Button
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Columns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Rows { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? ActionType { get; set; }

        public string ActionBody { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? Text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InternalBrowser? InternalBrowser { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OpenURLType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? OpenURLMediaType { get; set; }
    }
}
