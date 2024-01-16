using Newtonsoft.Json;

namespace eShopping.PromUaApi
{
    public class ProductImage
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
    }
}