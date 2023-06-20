using Newtonsoft.Json;

namespace eShop.PromUaApi
{
    public class GetProductsListResponse
    {
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        [JsonProperty("products")]
        public IEnumerable<Product> Products { get; set; }
    }
}
