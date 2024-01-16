using System.Reflection;

namespace eShopping.ViberBot.Framework.Strategies
{
    internal interface IStrategy
    {
        object?[] GetParameters(MethodInfo method, Callback callback);
        MethodInfo? PickControllerMethod();
    }
}
