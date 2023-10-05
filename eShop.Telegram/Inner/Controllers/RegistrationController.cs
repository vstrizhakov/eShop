using eShop.Telegram.Inner.Views;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Repositories;
using eShop.Telegram.Models;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Attributes;
using eShop.Messaging;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController]
    public class RegistrationController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly IProducer _producer;

        public RegistrationController(ITelegramService telegramService, ITelegramUserRepository telegramUserRepository, IProducer producer)
        {
            _telegramService = telegramService;
            _telegramUserRepository = telegramUserRepository;
            _producer = producer;
        }

        [TextMessage(Action = TelegramAction.RegisterClient, Command = "/start")]
        public async Task<ITelegramView?> ProcessAsync(TextMessageContext context, Guid providerId)
        {
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(context.FromId);
            if (telegramUser!.AccountId == null)
            {
                telegramUser.RegistrationProviderId = providerId;

                await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);

                return new FinishRegistrationView(context.ChatId);
            }
            else
            {
                return new AlreadyRegisteredView(context.ChatId);
            }
        }

        [ContactMessage]
        public async Task<ITelegramView?> ProcessAsync(ContactMessageContext context)
        {
            var contact = context.Contact;

            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            var providerId = user!.RegistrationProviderId;
            if (providerId != null)
            {
                var phoneNumber = contact.PhoneNumber;

                user.PhoneNumber = phoneNumber;
                user.RegistrationProviderId = null;

                await _telegramService.UpdateUserAsync(user);

                var internalMessage = new Messaging.Models.TelegramUserCreateAccountRequestMessage
                {
                    TelegramUserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = phoneNumber,
                    ProviderId = providerId.Value,
                };
                _producer.Publish(internalMessage);
            }

            return null;
        }
    }
}
