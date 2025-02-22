﻿using eShopping.TelegramFramework.Builders;
using Telegram.Bot;

namespace eShopping.TelegramFramework
{
    public interface ITelegramView
    {
        Task ProcessAsync(ITelegramBotClient botClient, IInlineKeyboardMarkupBuilder markupBuilder);
    }
}
