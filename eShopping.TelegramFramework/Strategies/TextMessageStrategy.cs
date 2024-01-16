using eShopping.TelegramFramework.Contexts;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShopping.TelegramFramework.Strategies
{
    internal class TextMessageStrategy : IStrategy
    {
        private readonly string _command;
        private readonly string _action;
        private readonly string?[] _parameters;

        public TextMessageStrategy(string command, string action, string?[] parameters)
        {
            _command = command;
            _action = action;
            _parameters = parameters;
        }

        public object?[] GetParameters(MethodInfo method, Update update)
        {
            var context = new TextMessageContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _parameters);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var method = ReflectionUtilities.FindControllerMethod<TextMessageAttribute>(attribute =>
                attribute.Command == _command && attribute.Action == _action);
            return method;
        }
    }
}
