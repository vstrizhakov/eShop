namespace eShop.Messaging.Models.Distribution.ResetPassword
{
    public class SendResetPasswordToTelegramMessage : Messaging.Message
    {
        public Guid TargetId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
