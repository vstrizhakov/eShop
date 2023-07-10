using eShop.Common;
using eShop.ViberBot;

namespace eShop.Viber.Services
{
    public class ViberBotConfigurationService : BackgroundService
    {
        private readonly IPublicUriBuilder _publicUriBuilder;
        private readonly IViberBotClient _botClient;

        public ViberBotConfigurationService(IPublicUriBuilder publicUriBuilder, IViberBotClient botClient)
        {
            _publicUriBuilder = publicUriBuilder;
            _botClient = botClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var webhookAddress = _publicUriBuilder.Path(ViberBotConfiguration.WebhookRoute);

            await _botClient.SetWebhookAsync(webhookAddress);
        }
    }
}
