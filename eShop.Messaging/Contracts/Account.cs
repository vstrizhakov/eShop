namespace eShop.Messaging.Contracts
{
    public class Account
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public Guid? TelegramUserId { get; set; }

        public Guid? ViberUserId { get; set; }

        public string? IdentityUserId { get; set; }
    }
}
