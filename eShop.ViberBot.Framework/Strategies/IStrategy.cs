using System.Reflection;

namespace eShop.ViberBot.Framework.Strategies
{
    internal interface IStrategy
    {
        object[] GetParameters(MethodInfo method, Callback callback);
        MethodInfo? PickControllerMethod();
    }
}
