using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace eShopping.ViberBot
{
    [JsonObject(NamingStrategyType = typeof(DefaultNamingStrategy))]
    public class CarouselContent : RichMediaBase
    {
        public override string Type { get; } = "rich_media";

    }
}
