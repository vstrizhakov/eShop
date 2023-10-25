using eShop.Messaging;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class GroupSettingsController
    {
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly IProducer _producer;

        public GroupSettingsController(ITelegramUserRepository telegramUserRepository, ITelegramChatRepository telegramChatRepository, IProducer producer)
        {
            _telegramUserRepository = telegramUserRepository;
            _telegramChatRepository = telegramChatRepository;
            _producer = producer;
        }

        [CallbackQuery(TelegramAction.SetGroupEnabled)]
        public async Task<ITelegramView?> DisableChat(CallbackQueryContext context, Guid telegramChatId, bool isEnabled)
        {
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(context.FromId);
            if (telegramUser!.AccountId != null)
            {
                var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(telegramChatId);
                if (telegramChat != null)
                {
                    var telegramChatSettings = telegramChat.Settings;
                    telegramChatSettings.IsEnabled = isEnabled;

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);

                    var internalEvent = new Messaging.Models.TelegramChatUpdatedEvent
                    {
                        AccountId = telegramUser.AccountId.Value,
                        TelegramChatId = telegramChatId,
                        IsEnabled = telegramChatSettings.IsEnabled,
                    };
                    _producer.Publish(internalEvent);

                    return new GroupSettingsView(context.ChatId, context.MessageId, telegramChat);
                }
                else
                {
                    // TODO:
                }
            }

            return null;
        }
    }
}
