namespace eShop.Messaging.Models.Distribution.ResetPassword
{
    public class SendResetPasswordMessage : Messaging.Message
    {
        public Guid AccountId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
