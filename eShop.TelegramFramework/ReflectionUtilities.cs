using eShop.TelegramFramework.Attributes;
using System.Reflection;

namespace eShop.TelegramFramework
{
    internal static class ReflectionUtilities
    {
        public static MethodInfo? FindControllerMethod<TAttribute>(Func<TAttribute, bool> filter) where TAttribute : Attribute
        {
            var method = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(e => e.GetCustomAttribute<TelegramControllerAttribute>() != null)
                .SelectMany(e => e.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                .FirstOrDefault(e =>
                {
                    var attribute = e.GetCustomAttribute<TAttribute>();
                    if (attribute == null)
                    {
                        return false;
                    }

                    return filter(attribute);
                });
            return method;
        }

        public static object[] MatchParameters(MethodInfo method, object context, params string[] inputParameters)
        {
            var parameters = method.GetParameters();

            var requiredParametersCount = parameters.Count(e => !e.IsOptional);
            var parametersCount = parameters.Length;
            var inputParametersCount = inputParameters.Length;
            if (requiredParametersCount > inputParametersCount + 1 || inputParametersCount + 1 > parametersCount)
            {
                throw new InvalidOperationException();
            }

            var methodParams = new object[parametersCount];
            methodParams[0] = context;

            for (int i = 0, j = 1; i < inputParametersCount; i++, j++)
            {
                var sourceParameterValue = inputParameters[i];

                var targetParameterInfo = parameters[j];
                var targetParameterType = targetParameterInfo.ParameterType;
                if (targetParameterType == typeof(string))
                {
                    methodParams[j] = sourceParameterValue;
                }
                else if (targetParameterType == typeof(Guid))
                {
                    if (Guid.TryParse(sourceParameterValue, out var guid))
                    {
                        methodParams[j] = guid;
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    try
                    {
                        var targetParameterValue = Convert.ChangeType(sourceParameterValue, targetParameterType);
                        methodParams[j] = targetParameterValue;
                    }
                    catch (InvalidCastException)
                    {
                        throw; // TODO: throw another exception maybe?
                    }
                }
            }

            return methodParams;
        }
    }
}
