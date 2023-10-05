using eShop.Telegram.Inner.Attributes;
using eShop.Telegram.Inner.Contexts;
using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Strategies
{
    public class ContactMessageStrategy : IStrategy
    {
        public object[] GetParameters(MethodInfo method, Update update)
        {
            var context = new ContactMessageContext(update);
            var parameters = ReflectionUtilities.MatchParameters(method, context);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var controller = ReflectionUtilities.FindControllerMethod<ContactMessageAttribute>(attribute => true);
            return controller;
        }
    }
}
