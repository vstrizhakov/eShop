using Newtonsoft.Json.Serialization;

namespace eShopping.ExchangeRate
{
    internal class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public SnakeCasePropertyNamesContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }
    }
}
