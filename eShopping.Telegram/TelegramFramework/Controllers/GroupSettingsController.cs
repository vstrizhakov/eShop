using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Repositories;
using MassTransit;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class GroupSettingsController
    {
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly IBus _producer;

        public GroupSettingsController(ITelegramUserRepository telegramUserRepository, ITelegramChatRepository telegramChatRepository, IBus producer)
        {
            _telegramUserRepository = telegramUserRepository;
            _telegramChatRepository = telegramChatRepository;
            _producer = producer;
        }

        [CallbackQuery(TelegramAction.SetGroupEnabled)]
        public async Task<ITelegramView?> SetGroupEnabled(CallbackQueryContext context, Guid telegramChatId, bool isEnabled)
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

                    var internalEvent = new Messaging.Contracts.TelegramChatUpdatedEvent
                    {
                        AccountId = telegramUser.AccountId.Value,
                        TelegramChatId = telegramChatId,
                        IsEnabled = telegramChatSettings.IsEnabled,
                    };
                    await _producer.Publish(internalEvent);

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
