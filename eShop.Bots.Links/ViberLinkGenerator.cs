using eShop.Bots.Common;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;

namespace eShop.Bots.Links
{
    internal class ViberLinkGenerator : IViberLinkGenerator
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly ViberLinkOptions _viberBotConfiguration;

        public ViberLinkGenerator(IBotContextConverter botContextConverter, IOptions<ViberLinkOptions> viberBotConfiguration)
        {
            _botContextConverter = botContextConverter;
            _viberBotConfiguration = viberBotConfiguration.Value;
        }

        public string Generate()
        {
            var inviteLink = QueryHelpers.AddQueryString($"viber://pa", new Dictionary<string, string?>()
            {
                { "chatURI", _viberBotConfiguration.ChatUrl },
            });

            return inviteLink;
        }

        public string Generate(string action, params string[] args)
        {
            var viberContext = _botContextConverter.Serialize(action, args);

            var inviteLink = QueryHelpers.AddQueryString($"viber://pa", new Dictionary<string, string?>()
            {
                { "chatURI", _viberBotConfiguration.ChatUrl },
                { "context", viberContext },
            });

            return inviteLink;
        }
    }
}
