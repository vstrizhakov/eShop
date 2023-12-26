using Newtonsoft.Json.Serialization;

namespace eShop.ExchangeRate
{
    internal class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }
    }
}
