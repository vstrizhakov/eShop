using Newtonsoft.Json;

namespace eShop.Identity.Models
{
    public class SignUpResponse
    {
        public bool Succeeded { get; set; }
        public ErrorCode? ErrorCode { get; set; }
    }
}
