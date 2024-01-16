using Newtonsoft.Json;

namespace eShopping.Identity.Models
{
    public class SignUpResponse
    {
        public bool Succeeded { get; set; }
        public ErrorCode? ErrorCode { get; set; }
    }
}
