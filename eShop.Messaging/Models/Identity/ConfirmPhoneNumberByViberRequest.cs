namespace eShop.Messaging.Models.Identity
{
    public class ConfirmPhoneNumberByViberRequest : Messaging.Message, IRequest<ConfirmPhoneNumberByViberResponse>
    {
        public Guid ViberUserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
    }
}
