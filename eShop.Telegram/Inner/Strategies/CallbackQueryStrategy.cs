﻿using eShop.Telegram.Inner.Contexts;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Strategies
{
    public class CallbackQueryStrategy : IStrategy
    {
        private readonly string _action;
        private readonly string[] _parameters;

        public CallbackQueryStrategy(string action, string[] parameters)
        {
            _action = action;
            _parameters = parameters;
        }

        public object[] GetParameters(MethodInfo method, Update update)
        {
            var context = new CallbackQueryContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _parameters);
            return parameters;
        }

        public Type? PickController()
        {
            var controller = ReflectionUtilities.FindController(attribute =>
                attribute.Context == TelegramContext.CallbackQuery && attribute.Action == _action);
            return controller;
        }
    }
}
