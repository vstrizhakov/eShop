using Microsoft.EntityFrameworkCore;

namespace eShop.Viber.Entities
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class ViberUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsSubcribed { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? RegistrationProviderId { get; set; }

        public ViberChatSettings ChatSettings { get; set; }
    }
}
