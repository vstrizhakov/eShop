using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace eShop.TelegramBot.Services
{
    public class ConfigureWebhookService : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private BotConfiguration _botConfiguration;

        public ConfigureWebhookService(IServiceProvider serviceProvider, IOptions<BotConfiguration> botConfiguration)
        {
            _serviceProvider = serviceProvider;
            _botConfiguration = botConfiguration.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            var webhookAddress = new Uri(_botConfiguration.Host, BotConfiguration.WebhookRoute);

            var res = await botClient.GetWebhookInfoAsync();
            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);

            var u = await botClient.GetUpdatesAsync();
            await botClient.SetWebhookAsync(
                webhookAddress.OriginalString,
                allowedUpdates: new[] { UpdateType.Message },
                cancellationToken: cancellationToken);

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
