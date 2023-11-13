using eShop.Messaging;
using eShop.Messaging.Models.Distribution.ResetPassword;
using eShop.Viber.Services;
using eShop.ViberBot;

namespace eShop.Viber.Handlers
{
    public class SendResetPasswordToViberMessageHandler : IMessageHandler<SendResetPasswordToViberMessage>
    {
        private readonly IViberService _viberService;
        private readonly IViberBotClient _botClient;

        public SendResetPasswordToViberMessageHandler(IViberService viberService, IViberBotClient botClient)
        {
            _viberService = viberService;
            _botClient = botClient;
        }

        public async Task HandleMessageAsync(SendResetPasswordToViberMessage message)
        {
            var user = await _viberService.GetUserByViberUserIdAsync(message.TargetId);
            if (user != null)
            {
                await _botClient.SendTextMessageAsync(user.ExternalId, null, message.ResetPasswordLink);
            }
        }
    }
}
