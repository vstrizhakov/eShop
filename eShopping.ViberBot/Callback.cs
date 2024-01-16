using Newtonsoft.Json;

namespace eShopping.ViberBot
{
    public class Callback
    {
        /// <summary>
        /// Callback type - which event triggered the callback
        /// </summary>
        [JsonProperty("event")]
        public EventType Event { get; set; }

        /// <summary>
        /// Time of the event that triggered the callback
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Unique ID of the message
        /// </summary>
        [JsonProperty("message_token")]
        public long MessageToken { get; set; }

        /// <summary>
        /// The specific type of <see cref="EventType.ConversationStarted"/> event
        /// </summary>
        [JsonProperty("type")]
        public ConversationStartedType? Type { get; set; }

        /// <summary>
        /// Any additional parameters added to the deep link used to access the conversation passed as a string. See <a href="https://developers.viber.com/docs/tools/deep-links">deep link</a> section for additional information
        /// </summary>
        [JsonProperty("context")]
        public string? Context { get; set; }

        [JsonProperty("user")]
        public User? User { get; set; }

        /// <summary>
        /// indicated whether a user is already subscribed
        /// </summary>
        [JsonProperty("subscribed")]
        public bool? Subscribed { get; set; }

        [JsonProperty("sender")]
        public User? Sender { get; set; }

        [JsonProperty("message")]
        public Message? Message { get; set; }

        [JsonProperty("user_id")]
        public string? UserId { get; set; }
    }
}
