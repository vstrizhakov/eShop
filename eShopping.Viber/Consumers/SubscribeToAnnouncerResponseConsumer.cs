using eShopping.Messaging.Contracts.Distribution;
using eShopping.Viber.ViberBotFramework.Views.Registration;
using eShopping.ViberBot.Framework;
using eShopping.Viber.Services;
using MassTransit;

namespace eShopping.Viber.Consumers
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
            var viberUserId = response.ViberUserId;
            if (viberUserId.HasValue)
            {
                var user = await _viberService.GetUserByViberUserIdAsync(viberUserId.Value);
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
}
