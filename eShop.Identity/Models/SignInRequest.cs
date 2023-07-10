using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace eShop.Identity.Models
{
    public class SignInRequest
    {
        [Required]
        [MaxLength(100)]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("remember")]
        public bool Remember { get; set; }

        [MaxLength(2000)]
        [JsonProperty("returnUrl")]
        public string ReturnUrl { get; set; }
    }
}
