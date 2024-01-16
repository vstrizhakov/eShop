using Newtonsoft.Json;

namespace eShopping.Identity.Models
{
    public class SignInResponse
    {
        public bool Succeeded { get; set; }
        public bool? ConfirmationRequired { get; set; }
        public string ValidReturnUrl { get; set; }
    }
}
