using Newtonsoft.Json;

namespace eShop.PromUaApi
{
    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }
}