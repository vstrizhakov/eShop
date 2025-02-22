﻿using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Services;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class MyGroupsController
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
                var chats = await _telegramService.GetManagableChats(user);

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
                var chats = await _telegramService.GetManagableChats(user);

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
                var chats = await _telegramService.GetManagableChats(user);
                var chat = chats.FirstOrDefault(e => e.Id == telegramChatId);
                if (chat != null)
                {
                    return new GroupSettingsView(context.ChatId, context.MessageId, chat);
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
