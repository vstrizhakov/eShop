using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace eShop.Identity.Entities
{
    [Index(nameof(AccountId), IsUnique = true)]
    public class User : IdentityUser
    {
        public Guid? AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}