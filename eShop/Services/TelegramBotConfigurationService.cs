using eShop.Configurations;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Services
{
    public class TelegramBotConfigurationService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPublicUriBuilder _publicUriBuilder;
        private readonly ILogger _logger;

        public TelegramBotConfigurationService(
            IServiceProvider serviceProvider,
            IPublicUriBuilder publicUriBuilder)
        {
            _serviceProvider = serviceProvider;
            _publicUriBuilder = publicUriBuilder;
            //_logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

                var webhookAddress = _publicUriBuilder.Path(TelegramBotConfiguration.WebhookRoute);

                try
                {
                    var rights = new ChatAdministratorRights
                    {
                        CanPostMessages = true,
                        CanManageChat = true,
                    };
                    await botClient.SetMyDefaultAdministratorRightsAsync(rights);
                    await botClient.SetMyDefaultAdministratorRightsAsync(rights, true);

                    await botClient.SetWebhookAsync(
                        webhookAddress,
                        allowedUpdates: new[]
                        {
                            UpdateType.Message,
                            UpdateType.ChannelPost,
                            UpdateType.CallbackQuery,
                        },
                        cancellationToken: cancellationToken);
                }
                catch (RequestException ex)
                {
                    //_logger.LogError(ex, "An error occurred while setting webhook");
                }
            });
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
