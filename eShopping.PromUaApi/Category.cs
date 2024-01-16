using Newtonsoft.Json;

namespace eShopping.PromUaApi
{
    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }
}