using System.Reflection;
using Telegram.Bot.Types;

namespace eShopping.TelegramFramework.Strategies
{
    internal interface IStrategy
    {
        object?[] GetParameters(MethodInfo method, Update update);
        MethodInfo? PickControllerMethod();
    }
}
