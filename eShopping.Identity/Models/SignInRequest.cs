using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace eShopping.Identity.Models
{
    public class SignInRequest
    {
        [Required]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        public bool Remember { get; set; }

        [MaxLength(2000)]
        public string? ReturnUrl { get; set; }
    }
}
