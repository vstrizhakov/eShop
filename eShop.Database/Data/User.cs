using Microsoft.AspNetCore.Identity;

namespace eShop.Database.Data
{
    public class User : IdentityUser
    {
        public string? ProviderId { get; set; }

        public User? Provider { get; set; }
        
        public IEnumerable<User> Clients { get; set; }
    }
}