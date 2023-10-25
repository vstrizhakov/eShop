using eShop.Messaging;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class RegistrationController
    {
        private readonly ITelegramService _telegramService;
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly IRequestClient _requestClient;
        private readonly ITelegramBotClient _botClient;

        public RegistrationController(
            ITelegramService telegramService,
            ITelegramUserRepository telegramUserRepository,
            IRequestClient requestClient,
            ITelegramBotClient botClient)
        {
            _telegramService = telegramService;
            _telegramUserRepository = telegramUserRepository;
            _requestClient = requestClient;
            _botClient = botClient;
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

                var request = new Messaging.Models.Telegram.RegisterTelegramUserRequest
                {
                    TelegramUserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = phoneNumber,
                    ProviderId = providerId.Value,
                };

                var response = await _requestClient.SendAsync(request);

                await _telegramService.SetAccountIdAsync(user, response.AccountId);

                var chatId = user.ExternalId;

                {
                    var replyMarkup = new ReplyKeyboardRemove();
                    var replyText = $"Вас успішно зареєстровано\n\n{response.ProviderEmail} встановлений як Ваш постачальник анонсів.";

                    await _botClient.SendTextMessageAsync(new ChatId(chatId), replyText, replyMarkup: replyMarkup);
                }

                var view = new WelcomeView(chatId, null);
                return view;
            }

            return null;
        }
    }
}
