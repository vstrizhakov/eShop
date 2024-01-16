using Newtonsoft.Json;

namespace eShopping.PromUaApi
{
    public class ProductPrice
    {
        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("minimum_order_quantity")]
        public float MinimumOrderQuantity { get; set; }
    }
}