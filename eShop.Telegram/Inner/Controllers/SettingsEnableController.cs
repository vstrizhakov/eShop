using eShop.Messaging;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.SettingsEnable, Context = TelegramContext.CallbackQuery)]
    public class SettingsEnableController : TelegramControllerBase
    {
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly IProducer _producer;

        public SettingsEnableController(ITelegramChatRepository telegramChatRepository, ITelegramUserRepository telegramUserRepository, IProducer producer)
        {
            _telegramChatRepository = telegramChatRepository;
            _telegramUserRepository = telegramUserRepository;
            _producer = producer;
        }

        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context, Guid telegramChatId)
        {
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(context.FromId);
            if (telegramUser!.AccountId != null)
            {
                var telegramChat = await _telegramChatRepository.GetTelegramChatByIdAsync(telegramChatId);
                if (telegramChat != null)
                {
                    var telegramChatSettings = telegramChat.Settings;
                    telegramChatSettings.IsEnabled = true;

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);

                    var internalEvent = new Messaging.Models.TelegramChatUpdatedEvent
                    {
                        AccountId = telegramUser.AccountId.Value,
                        TelegramChatId = telegramChatId,
                        IsEnabled = telegramChatSettings.IsEnabled,
                    };
                    _producer.Publish(internalEvent);

                    return new GroupSettingsView(context.ChatId, telegramChat);
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
