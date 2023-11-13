namespace eShop.Messaging.Models.Distribution.ResetPassword
{
    public class SendResetPasswordToViberMessage : Messaging.Message
    {
        public Guid RequestId { get; set; }
        public Guid TargetId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
