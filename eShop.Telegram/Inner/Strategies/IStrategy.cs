using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.Telegram.Inner.Strategies
{
    public interface IStrategy
    {
        object[] GetParameters(MethodInfo method, Update update);
        MethodInfo? PickControllerMethod();
    }
}
