using System.Reflection;
using Telegram.Bot.Types;

namespace eShop.TelegramFramework.Strategies
{
    internal interface IStrategy
    {
        object?[] GetParameters(MethodInfo method, Update update);
        MethodInfo? PickControllerMethod();
    }
}
