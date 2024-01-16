using System.ComponentModel.DataAnnotations;

namespace eShopping.Identity.Models
{
    public class RequestPasswordResetRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
