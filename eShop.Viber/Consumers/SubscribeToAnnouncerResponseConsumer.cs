using eShop.Messaging.Contracts.Distribution;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views.Registration;
using eShop.ViberBot.Framework;
using MassTransit;

namespace eShop.Viber.Consumers
{
    public class SubscribeToAnnouncerResponseConsumer : IConsumer<SubscribeToAnnouncerResponse>
    {
        private readonly IViberService _viberService;
        private readonly IViewRunner _viewRunner;

        public SubscribeToAnnouncerResponseConsumer(IViberService viberService, IViewRunner viewRunner)
        {
            _viberService = viberService;
            _viewRunner = viewRunner;
        }

        public async Task Consume(ConsumeContext<SubscribeToAnnouncerResponse> context)
        {
            var response = context.Message;
            var user = await _viberService.GetUserByAccountIdAsync(response.AccountId);
            if (user != null)
            {
                if (response.Succeeded)
                {
                    var view = new SubscribedToAnnouncerView(user.ExternalId, response.Announcer);
                    await _viewRunner.RunAsync(view);
                }
            }
        }
    }
}
