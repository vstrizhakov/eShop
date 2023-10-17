using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class MyGroupsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public MyGroupsController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [CallbackQuery(TelegramAction.MyGroups)]
        public async Task<ITelegramView?> GetGroups(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var chats = _telegramService.GetManagableChats(user);

                return new MyGroupsView(context.ChatId, context.MessageId, context.Id, chats);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.Refresh)]
        public async Task<ITelegramView?> RefreshGroups(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var chats = _telegramService.GetManagableChats(user);

                return new MyGroupsView(context.ChatId, context.MessageId, context.Id, chats);
            }
            else
            {
                // TODO: handle
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetUpGroup)]
        public async Task<ITelegramView?> SetUpGroup(CallbackQueryContext context, Guid telegramChatId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var chats = _telegramService.GetManagableChats(user);
                var telegramChat = chats.FirstOrDefault(e => e.ChatId == telegramChatId)?.Chat;
                if (telegramChat != null)
                {
                    if (telegramChat.Settings == null)
                    {
                        telegramChat.Settings = new TelegramChatSettings
                        {
                            Owner = user,
                        };

                        await _telegramService.UpdateUserAsync(user);
                    }

                    return new GroupSettingsView(context.ChatId, context.MessageId, telegramChat);
                }
                else
                {
                    // TODO:
                }
            }
            else
            {
                // TODO:
            }

            return null;
        }
    }
}
