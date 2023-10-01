using eShop.Telegram.Entities;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.SetUpGroup, Context = TelegramContext.CallbackQuery)]
    public class SetUpGroupController : TelegramControllerBase
    {
        private readonly ITelegramUserRepository _telegramUserRepository;

        public SetUpGroupController(ITelegramUserRepository telegramUserRepository)
        {
            _telegramUserRepository = telegramUserRepository;
        }

        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context, Guid telegramChatId)
        {
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(context.FromId);
            var telegramChat = telegramUser!.Chats.FirstOrDefault(e => e.ChatId == telegramChatId)?.Chat;
            if (telegramChat != null)
            {
                if (telegramChat.Settings == null)
                {
                    telegramChat.Settings = new TelegramChatSettings
                    {
                        Owner = telegramUser,
                    };

                    await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);
                }

                return new GroupSettingsView(context.ChatId, telegramChat);
            }
            else
            {
                // TODO:
            }

            return null;
        }
    }
}
