using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShopping.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy), ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Button
    {
        public int? Columns { get; set; }

        public int? Rows { get; set; }

        public string? ActionType { get; set; }

        public string ActionBody { get; set; }

        public string? Text { get; set; }

        public InternalBrowser? InternalBrowser { get; set; }

        public string? OpenURLType { get; set; }

        public string? OpenURLMediaType { get; set; }

        public string? BgColor { get; set; } = "#FFFFFF";
    }
}
