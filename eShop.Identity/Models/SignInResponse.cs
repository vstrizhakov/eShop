using Newtonsoft.Json;

namespace eShop.Identity.Models
{
    public class SignInResponse
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("validReturnUrl")]
        public string ValidReturnUrl { get; set; }
    }
}
