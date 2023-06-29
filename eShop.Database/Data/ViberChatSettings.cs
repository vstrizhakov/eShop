using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Data
{
    [PrimaryKey(nameof(OwnerId), nameof(ViberUserId))]
    public class ViberChatSettings
    {
        public string OwnerId { get; set; }
        public string ViberUserId { get; set; }
        public bool IsEnabled { get; set; }

        public ViberUser ViberUser { get; set; }
        public User Owner { get; set; }
    }
}
