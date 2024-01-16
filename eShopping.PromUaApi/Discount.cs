using Newtonsoft.Json;

namespace eShopping.PromUaApi
{
    public class Discount
    {
        [JsonProperty("value")]
        public float Value { get; set; }

        [JsonProperty("type")]
        public DiscountType Type { get; set; }

        [JsonProperty("date_start")]
        public string DateStart { get; set; }

        [JsonProperty("date_end")]
        public string DateEnd { get; set; }
    }
}