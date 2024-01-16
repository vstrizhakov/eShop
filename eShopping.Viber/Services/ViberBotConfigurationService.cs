using eShopping.Common;
using eShopping.ViberBot;

namespace eShopping.Viber.Services
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
            var webhookAddress = _publicUriBuilder.BackendPath(ViberBotConfiguration.WebhookRoute);

            await _botClient.SetWebhookAsync(webhookAddress);
        }
    }
}
