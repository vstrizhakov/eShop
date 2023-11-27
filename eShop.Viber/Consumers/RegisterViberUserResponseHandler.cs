using eShop.Messaging.Contracts.Viber;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views.Registration;
using eShop.ViberBot.Framework;
using MassTransit;

namespace eShop.Viber.Consumers
{
    public class RegisterViberUserResponseHandler : IConsumer<RegisterViberUserResponse>
    {
        private readonly IViberService _viberService;
        private readonly IViewRunner _viewRunner;

        public RegisterViberUserResponseHandler(IViberService viberService, IViewRunner viewRunner)
        {
            _viberService = viberService;
            _viewRunner = viewRunner;
        }

        public async Task Consume(ConsumeContext<RegisterViberUserResponse> context)
        {
            var response = context.Message;
            var user = await _viberService.GetUserByViberUserIdAsync(response.ViberUserId);
            if (user != null)
            {
                if (response.IsSuccess)
                {
                    await _viberService.SetAccountIdAsync(user, response.AccountId!.Value);

                    if (!response.IsConfirmationRequested)
                    {
                        var view = new SuccessfullyRegisteredView(user.ExternalId, response.ProviderEmail);
                        await _viewRunner.RunAsync(view);
                    }
                    else
                    {
                        var view = new PhoneNumberConfirmedView(user.ExternalId);
                        await _viewRunner.RunAsync(view);
                    }
                }
                else
                {
                    // TODO: handle
                }
            }
        }
    }
}
