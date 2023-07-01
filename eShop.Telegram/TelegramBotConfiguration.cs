namespace eShop.Telegram
{
    public class TelegramBotConfiguration
    {
        public const string WebhookRoute = "/api/telegram/webhook";

        public string Username { get; set; }
        public string Token { get; set; }
    }
}
