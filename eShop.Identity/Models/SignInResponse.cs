using Newtonsoft.Json;

namespace eShop.Identity.Models
{
    public class SignInResponse
    {
        public bool Succeeded { get; set; }
        public bool? ConfirmationRequired { get; set; }
        public string ValidReturnUrl { get; set; }
    }
}
