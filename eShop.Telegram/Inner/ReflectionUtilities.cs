using eShop.Telegram.Inner.Controllers;
using System.Reflection;

namespace eShop.Telegram.Inner
{
    public static class ReflectionUtilities
    {
        public static Type? FindController(Func<TelegramControllerAttribute, bool> filter)
        {
            var controller = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(e => e.IsSubclassOf(typeof(TelegramControllerBase)))
                .FirstOrDefault(e =>
                {
                    var attribute = e.GetCustomAttribute<TelegramControllerAttribute>();
                    if (attribute == null)
                    {
                        return false;
                    }

                    return filter(attribute);
                });
            return controller;
        }

        public static object[] MatchParameters(MethodInfo method, object context, params string[] inputParameters)
        {
            var parameters = method.GetParameters();
            var parametersCount = parameters.Length;

            if (parametersCount != (inputParameters?.Length ?? 0) + 1)
            {
                throw new InvalidOperationException();
            }

            var methodParams = new object[parametersCount];
            methodParams[0] = context;

            for (int i = 1; i < parametersCount; i++)
            {
                var parameterValue = inputParameters[i - 1];

                var parameter = parameters[i];
                var parameterType = parameter.ParameterType;
                if (parameterType == typeof(string))
                {
                    methodParams[i] = parameterValue;
                }
                else if (parameterType == typeof(Guid))
                {
                    if (Guid.TryParse(parameterValue, out var guid))
                    {
                        methodParams[i] = guid;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            return methodParams;
        }
    }
}
