using System.ComponentModel.DataAnnotations;

namespace eShopping.Identity.Models
{
    public class CompleteResetPasswordRequest
    {
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
