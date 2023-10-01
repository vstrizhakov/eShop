using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.MyGroups, Context = TelegramContext.CallbackQuery)]
    public class MyGroupsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public MyGroupsController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var chats = _telegramService.GetManagableChats(user);

                return new MyGroupsView(context.ChatId, chats);
            }

            return null;
        }
    }
}
