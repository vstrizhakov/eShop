namespace eShopping.Messaging.Contracts.Distribution.ResetPassword
{
    public class SendResetPasswordToViberMessage
    {
        public Guid RequestId { get; set; }
        public Guid TargetId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
