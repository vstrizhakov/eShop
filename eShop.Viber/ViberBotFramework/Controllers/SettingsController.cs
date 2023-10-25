using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class SettingsController
    {
        private readonly IViberService _viberService;

        public SettingsController(IViberService viberService)
        {
            _viberService = viberService;
        }

        [TextMessage(Action = ViberContext.Settings)]
        public async Task<IViberView?> Settings(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                var isEnabled = user.ChatSettings.IsEnabled;

                return new SettingsView(user.ExternalId, isEnabled);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SettingsEnable)]
        public async Task<IViberView?> SettingsEnable(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                await _viberService.SetIsChatEnabledAsync(user, true);

                return new SettingsView(user.ExternalId, true);
            }

            return null;
        }

        [TextMessage(Action = ViberContext.SettingsDisable)]
        public async Task<IViberView?> SettingsDisable(TextMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId != null)
            {
                await _viberService.SetIsChatEnabledAsync(user, false);

                return new SettingsView(user.ExternalId, false);
            }

            return null;
        }
    }
}
