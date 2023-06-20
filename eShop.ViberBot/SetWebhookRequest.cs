using Newtonsoft.Json;

namespace eShop.ViberBot
{
    internal class SetWebhookRequest
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("event_types", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<EventType>? EventTypes { get; set; }

        [JsonProperty("send_name")]
        public bool SendName { get; set; }

        [JsonProperty("send_photo")]
        public bool SendPhoto { get; set; }
    }
}
