namespace eShopping.Messaging.Contracts.Distribution.ResetPassword
{
    public class SendResetPasswordMessage
    {
        public Guid AccountId { get; set; }
        public string ResetPasswordLink { get; set; }
    }
}
