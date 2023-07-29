using eShop.Bots.Common;
using eShop.Telegram.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShop.Telegram.Services
{
    public class TelegramInvitationLinkGenerator : ITelegramInvitationLinkGenerator
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly TelegramBotConfiguration _telegramBotConfiguration;

        public TelegramInvitationLinkGenerator(IBotContextConverter botContextConverter, IOptions<TelegramBotConfiguration> options)
        {
            _botContextConverter = botContextConverter;
            _telegramBotConfiguration = options.Value;
        }

        public string Generate(Guid providerId)
        {
            var context = _botContextConverter.Serialize(TelegramContext.Action.RegisterClient, providerId.ToString());

            var link = QueryHelpers.AddQueryString($"https://t.me/{_telegramBotConfiguration.Username}", "start", context);
            return link;
        }
    }
}
