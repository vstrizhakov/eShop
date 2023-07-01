using Azure;
using eShop.Bots.Common;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Models;
using eShop.Viber.Repositories;
using eShop.ViberBot;

namespace eShop.Viber.MessageHandlers
{
    public class ViberUserCreateAccountUpdateMessageHandler : IMessageHandler<ViberUserCreateAccountUpdateMessage>
    {
        private readonly IViberBotClient _botClient;
        private readonly IBotContextConverter _botContextConverter;
        private readonly IViberUserRepository _repository;

        public ViberUserCreateAccountUpdateMessageHandler(
            IViberBotClient botClient,
            IBotContextConverter botContextConverter,
            IViberUserRepository repository)
        {
            _botClient = botClient;
            _botContextConverter = botContextConverter;
            _repository = repository;
        }

        public async Task HandleMessageAsync(ViberUserCreateAccountUpdateMessage message)
        {
            if (message.IsSuccess)
            {
                var viberUser = await _repository.GetViberUserByIdAsync(message.ViberUserId.Value);
                if (viberUser != null)
                {
                    await _repository.UpdateAccountIdAsync(viberUser, message.AccountId.Value);

                    var replyText = $"{message.ProviderEmail} встановлений як Ваш постачальник анонсів.";
                    var keyboard = new Keyboard
                    {
                        Buttons = new[]
                        {
                            new Button
                            {
                                Rows = 1,
                                Text = "Увімкнути",
                                ActionBody = _botContextConverter.Serialize(ViberContext.SettingsEnable),
                            },
                        },
                    };

                    await _botClient.SendTextMessageAsync(viberUser.ExternalId, null, replyText, minApiVersion: 7, keyboard: keyboard);
                }
            }
            else
            {

            }
        }
    }
}
