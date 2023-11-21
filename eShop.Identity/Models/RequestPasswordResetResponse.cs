namespace eShop.Identity.Models
{
    public class RequestPasswordResetResponse
    {
        public bool Succeeded { get; set; }
        public ErrorCode? ErrorCode { get; set; }
    }
}
