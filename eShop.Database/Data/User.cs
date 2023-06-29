using Microsoft.AspNetCore.Identity;

namespace eShop.Database.Data
{
    public class User : IdentityUser
    {
        public string? ProviderId { get; set; }

        public User? Provider { get; set; }

        public ICollection<User> Clients { get; set; } = new List<User>();

        public ICollection<TelegramChatSettings> TelegramChats { get; set; } = new List<TelegramChatSettings>();
        public TelegramUser? TelegramUser { get; set; }

        public ViberChatSettings? ViberChatSettings { get; set; }
        public ViberUser? ViberUser { get; set; }
    }
}