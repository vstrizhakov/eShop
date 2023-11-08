using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views.Registration;
using eShop.ViberBot.Framework;

namespace eShop.Viber.Handlers
{
    public class ConfirmPhoneNumberByViberResponseHandler : IMessageHandler<ConfirmPhoneNumberByViberResponse>
    {
        private readonly IViewRunner _viewRunner;
        private readonly IViberService _viberService;

        public ConfirmPhoneNumberByViberResponseHandler(
            IViewRunner viewRunner,
            IViberService viberService)
        {
            _viewRunner = viewRunner;
            _viberService = viberService;
        }

        public async Task HandleMessageAsync(ConfirmPhoneNumberByViberResponse response)
        {
            var user = await _viberService.GetUserByViberUserIdAsync(response.ViberUserId);
            if (user != null)
            {
                if (response.Succeeded)
                {
                    await _viberService.SetActiveContextAsync(user, null);

                    if (!user.AccountId.HasValue)
                    {
                        await _viberService.SetAccountIdAsync(user, response.AccountId!.Value);
                    }

                    var view = new SuccessfullyRegisteredView(user.ExternalId, null);
                    await _viewRunner.RunAsync(view);
                }
                else
                {
                    if (response.IsPhoneNumberInvalid == true)
                    {
                        var view = new PhoneNumberInvalidView(user.ExternalId);
                        await _viewRunner.RunAsync(view);
                    }
                    else if (response.IsTokenInvalid == true)
                    {
                        var view = new TokenInvalidView(user.ExternalId);
                        await _viewRunner.RunAsync(view);
                    }
                }
            }
            else
            {
                // TODO: handle viber user is absent
            }
        }
    }
}
