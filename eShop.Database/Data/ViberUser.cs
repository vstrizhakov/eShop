using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Data
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class ViberUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public bool IsSubcribed { get; set; }
    }
}
