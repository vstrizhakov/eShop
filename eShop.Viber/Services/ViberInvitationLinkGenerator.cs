using eShop.Bots.Common;
using eShop.Viber.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShop.Viber.Services
{
    public class ViberInvitationLinkGenerator : IViberInvitationLinkGenerator
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly ViberBotConfiguration _viberBotConfiguration;

        public ViberInvitationLinkGenerator(IBotContextConverter botContextConverter, IOptions<ViberBotConfiguration> viberBotConfiguration)
        {
            _botContextConverter = botContextConverter;
            _viberBotConfiguration = viberBotConfiguration.Value;
        }

        public string Generate(Guid providerId)
        {
            var viberContext = _botContextConverter.Serialize(ViberContext.RegisterClient, providerId.ToString());

            var inviteLink = QueryHelpers.AddQueryString($"viber://pa", new Dictionary<string, string?>()
            {
                { "chatURI", _viberBotConfiguration.ChatUrl },
                { "context", viberContext },
            });

            return inviteLink;
        }
    }
}
