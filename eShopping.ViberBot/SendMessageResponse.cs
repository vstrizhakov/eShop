using Newtonsoft.Json;

namespace eShopping.ViberBot
{
    internal class SendMessageResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("status_message")]
        public string? StatusMessage { get; set; }

        [JsonProperty("message_token")]
        public long? MessageToken { get; set; }
    }
}
