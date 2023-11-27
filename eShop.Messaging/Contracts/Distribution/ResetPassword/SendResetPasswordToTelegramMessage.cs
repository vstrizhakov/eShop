namespace eShop.Messaging.Contracts.Distribution.ResetPassword
{
    public class SendResetPasswordToTelegramMessage
    {
        public Guid TargetId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
