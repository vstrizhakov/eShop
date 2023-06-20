namespace eShop.Database.Data
{
    public class TelegramRequest
    {
        public int Id { get; set; }

        public TelegramRequestType Type { get; set; }

        public string UserId { get; set;}

        public TelegramUser User { get; set; }
    }
}
