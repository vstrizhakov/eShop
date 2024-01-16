using Newtonsoft.Json;

namespace eShopping.ViberBot
{
    public class Contact
    {
        /// <summary>
        /// Name of the contact
        /// 
        /// required. Max 28 characters
        /// </summary>
        [JsonProperty("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Phone number of the contact
        /// 
        /// required. Max 18 characters
        /// </summary>
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
    }
}
