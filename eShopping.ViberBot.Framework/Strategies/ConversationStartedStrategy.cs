using eShopping.ViberBot.Framework.Contexts;
using eShopping.ViberBot.Framework;
using eShopping.ViberBot.Framework.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace eShopping.ViberBot.Framework.Strategies
{
    internal class ConversationStartedStrategy : IStrategy
    {
        private readonly string _action;
        private readonly string[] _parameters;

        public ConversationStartedStrategy(string action, string[] parameters)
        {
            _action = action;
            _parameters = parameters;
        }

        public object?[] GetParameters(MethodInfo method, Callback callback)
        {
            var context = new ConversationStartedContext(callback);
            var parameters = ReflectionUtilities.MatchParameters(method, context, _parameters);
            return parameters;
        }

        public MethodInfo? PickControllerMethod()
        {
            var method = ReflectionUtilities.FindControllerMethod<ConversationStartedAttribute>(attribute
                => attribute.Action == _action);
            return method;
        }
    }
}
