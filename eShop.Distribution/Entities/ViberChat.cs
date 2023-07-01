using Microsoft.EntityFrameworkCore;

namespace eShop.Distribution.Entities
{
    [PrimaryKey(nameof(AccountId), nameof(ViberUserId))]
    public class ViberChat
    {
        public Guid AccountId { get; set; }
        public Guid ViberUserId { get; set; }
        public bool IsEnabled { get; set; }

        public Account Account { get; set; }
    }
}
