using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.TelegramFramework.Strategies
{
    internal class CallbackQueryStrategy : IStrategy
    {
        private readonly string _action;
        private readonly string?[] _parameters;

        public CallbackQueryStrategy(string action, string?[] parameters)
        {
            _action = action;
            _parameters = parameters;
        }

        public object?[] GetParameters(MethodInfo method, Update update)
        {
            var context = new CallbackQueryContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _parameters);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var method = ReflectionUtilities.FindControllerMethod<CallbackQueryAttribute>(attribute
                => attribute.Action == _action);
            return method;
        }
    }
}
