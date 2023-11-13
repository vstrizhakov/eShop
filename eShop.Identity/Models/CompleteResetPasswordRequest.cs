using System.ComponentModel.DataAnnotations;

namespace eShop.Identity.Models
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
