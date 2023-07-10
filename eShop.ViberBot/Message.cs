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
        public int? MinApiVersion { get; set; }

        /// <summary>
        /// The text of the message
        /// 
        /// required. Max length 7,000 characters
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string? Text { get; set; }

        /// <summary>
        /// URL of the image (JPEG, PNG, non-animated GIF)
        /// 
        /// required. The URL must have a resource with a .jpeg, .png or .gif file extension as the last path segment. Example: http://www.example.com/path/image.jpeg. Animated GIFs can be sent as URL messages or file messages. Max image size: 1MB on iOS, 3MB on Android.
        /// </summary>
        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public string? Media { get; set; }

        /// <summary>
        /// URL of a reduced size image (JPEG, PNG, GIF)
        /// 
        /// optional. Recommended: 400x400. Max size: 100kb.
        /// </summary>
        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string? Thumbnail { get; set; }

        [JsonProperty("rich_media", NullValueHandling = NullValueHandling.Ignore)]
        public CarouselContent? CarouselContent { get; set; }

        [JsonProperty("keyboard", NullValueHandling = NullValueHandling.Ignore)]
        public Keyboard? Keyboard { get; set; }

        [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
        public Contact? Contact { get; set; }
    }
}
