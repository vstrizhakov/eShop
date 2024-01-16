using eShopping.Messaging.Contracts.Distribution.ResetPassword;
using eShopping.ViberBot;
using eShopping.Viber.Services;
using MassTransit;

namespace eShopping.Viber.Consumers
{
    public class SendResetPasswordToViberMessageHandler : IConsumer<SendResetPasswordToViberMessage>
    {
        private readonly IViberService _viberService;
        private readonly IViberBotClient _botClient;

        public SendResetPasswordToViberMessageHandler(IViberService viberService, IViberBotClient botClient)
        {
            _viberService = viberService;
            _botClient = botClient;
        }

        public async Task Consume(ConsumeContext<SendResetPasswordToViberMessage> context)
        {
            var command = context.Message;
            var user = await _viberService.GetUserByViberUserIdAsync(command.TargetId);
            if (user != null)
            {
                await _botClient.SendTextMessageAsync(user.ExternalId, null, command.ResetPasswordLink);
            }
        }
    }
}
