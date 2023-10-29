using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class ComissionSettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient _requestClient;
        private readonly IBotContextConverter _botContextConverter;

        public ComissionSettingsController(IViberService viberService, IRequestClient requestClient, IBotContextConverter botContextConverter)
        {
            _viberService = viberService;
            _requestClient = requestClient;
            _botContextConverter = botContextConverter;
        }

        [TextMessage(Action = ViberContext.ComissionSettings)]
        public async Task<IViberView?> ComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionSettingsRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                return new ComissionSettingsView(user.ExternalId, response.Amount);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SetComissionAmount)]
        public async Task<IViberView?> GetComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionAmountRequest(user.AccountId.Value);
                var response = await _requestClient.SendAsync(request);

                var activeContext = _botContextConverter.Serialize(ViberContext.SetComissionAmount);
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new SetComissionAmountView(user.ExternalId, response.Amount);
            }

            return null;
        }

        [TextMessage(ActiveAction = ViberContext.SetComissionAmount)]
        public async Task<IViberView?> SetComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                if (double.TryParse(context.Text, out var amount))
                {
                    var request = new SetComissionAmountRequest(user.AccountId.Value, amount);
                    var response = await _requestClient.SendAsync(request);

                    await _viberService.SetActiveContextAsync(user, null);

                    return new ComissionSettingsView(user.ExternalId, response.Amount);
                }
            }

            return null;
        }
    }
}
