using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;
using System.Reflection;

namespace eShop.ViberBot.Framework.Strategies
{
    internal class ContactMessageStrategy : IStrategy
    {
        public object[] GetParameters(MethodInfo method, Callback callback)
        {
            var context = new ContactMessageContext(callback);
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
