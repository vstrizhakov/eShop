using eShopping.Bots.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShopping.Bots.Links
{
    internal class TelegramLinkGenerator : ITelegramLinkGenerator
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly TelegramLinkOptions _options;

        public TelegramLinkGenerator(IBotContextConverter botContextConverter, IOptions<TelegramLinkOptions> options)
        {
            _botContextConverter = botContextConverter;
            _options = options.Value;
        }

        public string Generate()
        {
            var link = $"https://t.me/{_options.Username}";
            return link;
        }

        public string Generate(string action, params string[] args)
        {
            var context = _botContextConverter.Serialize(action, args);

            var link = QueryHelpers.AddQueryString($"https://t.me/{_options.Username}", "start", context);
            return link;
        }
    }
}
