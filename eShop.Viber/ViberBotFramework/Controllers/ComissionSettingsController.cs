using eShop.Bots.Common;
using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;
using MassTransit;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class ComissionSettingsController
    {
        private readonly IViberService _viberService;
        private readonly IRequestClient<GetComissionSettingsRequest> _getComissionSettingsRequestClient;
        private readonly IRequestClient<SetComissionAmountRequest> _setComissionAmounRequestClient;
        private readonly IRequestClient<GetComissionAmountRequest> _getComissionAmountRequestClient;
        private readonly IBotContextConverter _botContextConverter;

        public ComissionSettingsController(
            IViberService viberService,
            IBotContextConverter botContextConverter,
            IRequestClient<GetComissionSettingsRequest> getComissionSettingsRequestClient,
            IRequestClient<GetComissionAmountRequest> getComissionAmountRequestClient,
            IRequestClient<SetComissionAmountRequest> setComissionAmounRequestClient)
        {
            _viberService = viberService;
            _botContextConverter = botContextConverter;
            _getComissionSettingsRequestClient = getComissionSettingsRequestClient;
            _getComissionAmountRequestClient = getComissionAmountRequestClient;
            _setComissionAmounRequestClient = setComissionAmounRequestClient;
        }

        [TextMessage(Action = ViberAction.ComissionSettings)]
        public async Task<IViberView?> ComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionSettingsRequest(user.AccountId.Value);
                var response = await _getComissionSettingsRequestClient.GetResponse<GetComissionSettingsResponse>(request);

                return new ComissionSettingsView(user.ExternalId, response.Message.Amount);
            }

            return null;
        }

        [TextMessage(Action = ViberAction.SetComissionAmount)]
        public async Task<IViberView?> GetComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var request = new GetComissionAmountRequest(user.AccountId.Value);
                var response = await _getComissionAmountRequestClient.GetResponse<GetComissionAmountResponse>(request);

                var activeContext = _botContextConverter.Serialize(ViberAction.SetComissionAmount);
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new SetComissionAmountView(user.ExternalId, response.Message.Amount);
            }

            return null;
        }

        [TextMessage(ActiveAction = ViberAction.SetComissionAmount)]
        public async Task<IViberView?> SetComissionSettings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                if (decimal.TryParse(context.Text, out var amount))
                {
                    var request = new SetComissionAmountRequest(user.AccountId.Value, amount);
                    var response = await _setComissionAmounRequestClient.GetResponse<SetComissionAmountResponse>(request);

                    await _viberService.SetActiveContextAsync(user, null);

                    return new ComissionSettingsView(user.ExternalId, response.Message.Amount);
                }
            }

            return null;
        }
    }
}
