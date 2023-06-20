using Microsoft.EntityFrameworkCore;

namespace eShop.Database.Data
{
    [Index(nameof(ExternalId), IsUnique = true)]
    public class TelegramUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Username { get; set; }

        public long ExternalId { get; set; }

        public string? LinkedUserId { get; set; }

        public ICollection<TelegramChatMember> Chats { get; set; } = new List<TelegramChatMember>();
        public User? LinkedUser { get; set; }
    }
}
