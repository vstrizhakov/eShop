using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShop.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public abstract class RichMediaBase
    {
        public abstract string Type { get; }

        public IEnumerable<Button> Buttons { get; set; }
    }
}
