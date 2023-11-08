namespace eShop.Messaging.Models.Identity
{
    public class ConfirmPhoneNumberByViberResponse : Messaging.Message, IResponse
    {
        public Guid ViberUserId { get; set; }
        public bool Succeeded { get; set; }
        public Guid? AccountId { get; set; }
        public bool? IsPhoneNumberInvalid { get; set; }
        public bool? IsTokenInvalid { get; set; }
    }
}
