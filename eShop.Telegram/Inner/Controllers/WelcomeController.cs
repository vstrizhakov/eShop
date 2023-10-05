using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(Context = TelegramContext.TextMessage, Command = "/start")]
    public class WelcomeController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public WelcomeController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        public async Task<ITelegramView?> ProcessAsync(TextMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                return new WelcomeView(context.ChatId);
            }

            return null;
        }
    }
}
