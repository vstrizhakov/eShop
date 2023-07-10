using eShop.Database.Data;

namespace eShop.MVC.Models.Clients
{
    public class IndexModel
    {
        public IEnumerable<User> Clients { get; set; }
        public string TelegramInviteLink { get; set; }
        public string ViberInviteLink { get; set; }
    }
}
