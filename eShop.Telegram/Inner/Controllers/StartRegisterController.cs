using eShop.Telegram.Inner.Views;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using eShop.Telegram.Repositories;
using eShop.Telegram.Models;
using eShop.Telegram.Inner.Contexts;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(TelegramAction.RegisterClient, Context = TelegramContext.TextMessage, Command = "/start")]
    public class StartRegisterController : TelegramControllerBase
    {
        private readonly ITelegramUserRepository _telegramUserRepository;

        public StartRegisterController(ITelegramUserRepository telegramUserRepository)
        {
            _telegramUserRepository = telegramUserRepository;
        }

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
    }
}
