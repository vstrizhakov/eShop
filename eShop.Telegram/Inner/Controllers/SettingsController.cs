﻿using eShop.Telegram.Inner.Attributes;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Models;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController]
    public class SettingsController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;

        public SettingsController(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        [CallbackQuery(TelegramAction.Settings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                return new SettingsView(context.ChatId);
            }

            return null;
        }
    }
}
