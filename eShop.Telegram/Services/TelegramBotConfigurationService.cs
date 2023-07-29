using eShop.Common;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Services
{
    public class TelegramBotConfigurationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IPublicUriBuilder _publicUriBuilder;

        public TelegramBotConfigurationService(
            IServiceProvider serviceProvider,
            IPublicUriBuilder publicUriBuilder)
        {
            _serviceProvider = serviceProvider;
            _publicUriBuilder = publicUriBuilder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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
                    });
            }
            catch (RequestException ex)
            {
                //_logger.LogError(ex, "An error occurred while setting webhook");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            using var scope = _serviceProvider.CreateScope();
            var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

            await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }
    }
}
