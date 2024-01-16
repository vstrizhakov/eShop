using eShopping.Database;

namespace eShopping.Identity.Entities
{
    public class User : EntityBase
    {
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? AccountId { get; set; }
        public string? PasswordHash { get; set; }

        protected override string GetPartitionKey()
        {
            return UseDiscriminator();
        }
    }
}