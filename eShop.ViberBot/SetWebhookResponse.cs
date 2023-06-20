using Newtonsoft.Json;

namespace eShop.ViberBot
{
    internal class SetWebhookResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("status_message")]
        public string? StatusMessage { get; set; }

        [JsonProperty("event_types")]
        public IEnumerable<EventType>? EventTypes { get; set; }
    }
}
