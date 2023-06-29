using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShop.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class Keyboard : RichMediaBase
    {
        public override string Type { get; } = "keyboard";
    }
}
