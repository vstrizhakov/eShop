using Microsoft.EntityFrameworkCore;

namespace eShop.Viber.Entities
{
    [PrimaryKey(nameof(ViberUserId))]
    public class ViberChatSettings
    {
        public Guid ViberUserId { get; set; }
        public bool IsEnabled { get; set; }

        public ViberUser ViberUser { get; set; }
    }
}
