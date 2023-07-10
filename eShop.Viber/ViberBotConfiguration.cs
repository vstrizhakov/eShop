namespace eShop.Viber
{
    public class ViberBotConfiguration
    {
        public const string WebhookRoute = "/api/viber/webhook";

        public string ChatUrl { get; set; }
        public string Token { get; set; }
    }
}
