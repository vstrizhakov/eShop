using eShop.Telegram.Inner.Contexts;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Strategies
{
    public class TextMessageStrategy : IStrategy
    {
        private readonly string _command;
        private readonly string _action;
        private readonly string[] _parameters;

        public TextMessageStrategy(string command, string action, string[] parameters)
        {
            _command = command;
            _action = action;
            _parameters = parameters;
        }

        public object[] GetParameters(MethodInfo method, Update update)
        {
            var context = new TextMessageContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _parameters);
            return parameters;
        }

        public Type? PickController()
        {
            var controller = ReflectionUtilities.FindController(attribute =>
                attribute.Context == TelegramContext.TextMessage && attribute.Command == _command && attribute.Action == _action);
            return controller;
        }
    }
}
