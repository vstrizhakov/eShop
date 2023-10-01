using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Repositories;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.Refresh, Context = TelegramContext.CallbackQuery)]
    public class RefreshController : TelegramControllerBase
    {
        private readonly ITelegramUserRepository _telegramUserRepository;

        public RefreshController(ITelegramUserRepository telegramUserRepository)
        {
            _telegramUserRepository = telegramUserRepository;
        }

        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(context.FromId);
            if (telegramUser!.AccountId != null)
            {
                var telegramUserChats = telegramUser.Chats
                    .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                    .Where(e => e.Chat.SupergroupId == null)
                    .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);

                return new RefreshView(context.ChatId, telegramUserChats);
            }
            else
            {
                // TODO: handle
            }

            return null;
        }
    }
}
