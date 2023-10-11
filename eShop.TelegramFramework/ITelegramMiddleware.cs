﻿using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    public interface ITelegramMiddleware
    {
        Task HandleUpdateAsync(Update update);
    }
}
