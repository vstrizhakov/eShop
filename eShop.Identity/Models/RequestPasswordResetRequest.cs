using System.ComponentModel.DataAnnotations;

namespace eShop.Identity.Models
{
    public class RequestPasswordResetRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
}
