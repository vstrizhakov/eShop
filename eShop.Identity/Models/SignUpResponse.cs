using Newtonsoft.Json;

namespace eShop.Identity.Models
{
    public class SignUpResponse
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
    }
}
