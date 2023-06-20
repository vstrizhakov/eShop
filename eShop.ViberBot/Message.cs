using Newtonsoft.Json;

namespace eShop.ViberBot
{
    public class Message
    {
        /// <summary>
        /// Unique Viber user id
        /// 
        /// required, subscribed valid user id
        /// </summary>
        [JsonProperty("receiver", NullValueHandling = NullValueHandling.Ignore)]
        public string? Receiver { get; set; }

        /// <summary>
        /// Message type
        /// 
        /// required. Available message types: text, picture, video, file, location, contact, sticker, carousel content and url
        /// </summary>
        [JsonProperty("type")]
        public MessageType Type { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public User? Sender { get; set; }

        /// <summary>
        /// Allow the account to track messages and user’s replies. Sent tracking_data value will be passed back with user’s reply
        /// 
        /// optional. max 4000 characters
        /// </summary>
        [JsonProperty("tracking_data", NullValueHandling = NullValueHandling.Ignore)]
        public string? TrackingData { get; set; }

        /// <summary>
        /// Minimal API version required by clients for this message (default 1)
        /// 
        /// optional. client version support the API version. Certain features may not work as expected if set to a number that’s below their requirements.
        /// </summary>
        [JsonProperty("min_api_version", NullValueHandling = NullValueHandling.Ignore)]
        public string? MinApiVersion { get; set; }

        /// <summary>
        /// The text of the message
        /// 
        /// required. Max length 7,000 characters
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string? Text { get; set; }

        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public string? Media { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string? Thumbnail { get; set; }
    }
}
