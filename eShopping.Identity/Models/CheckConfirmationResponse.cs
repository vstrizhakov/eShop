namespace eShopping.Identity.Models
{
    public class CheckConfirmationResponse
    {
        public bool Confirmed { get; set; }
        public ConfirmationLinks? Links { get; set; }
        public string? ValidReturnUrl { get; set; }
    }
}
