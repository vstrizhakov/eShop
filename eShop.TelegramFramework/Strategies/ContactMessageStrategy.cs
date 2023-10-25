using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.TelegramFramework.Strategies
{
    internal class ContactMessageStrategy : IStrategy
    {
        public object[] GetParameters(MethodInfo method, Update update)
        {
            var context = new ContactMessageContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var method = ReflectionUtilities.FindControllerMethod<ContactMessageAttribute>(attribute => true);
            return method;
        }
    }
}
