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

        public Type? PickController()
        {
            var controller = ReflectionUtilities.FindController(attribute =>
                attribute.Context == TelegramContext.ContactMessage);
            return controller;
        }
    }
}
