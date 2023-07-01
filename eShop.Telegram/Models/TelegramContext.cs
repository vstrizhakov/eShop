namespace eShop.Telegram.Models
{
    public static class TelegramContext
    {
        public static class Action
        {
            public const string RegisterClient = "rc";
            public const string SetUpGroup = "sug";
            public const string Refresh = "rh";
            public const string SettingsDisable = "sd";
            public const string SettingsEnable = "se";
        }
    }
}
