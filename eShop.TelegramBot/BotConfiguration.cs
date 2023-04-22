namespace eShop.TelegramBot
{
    public class BotConfiguration
    {
        public const string WebhookRoute = "/api/telegram/webhook";

        public Uri Host { get; set; }
        public string BotToken { get; set; }
    }
}
