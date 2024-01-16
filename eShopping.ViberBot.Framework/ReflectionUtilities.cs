using eShopping.ViberBot.Framework.Attributes;
using System.Reflection;

namespace eShopping.ViberBot.Framework
{
    internal static class ReflectionUtilities
    {
        public static MethodInfo? FindControllerMethod<TAttribute>(Func<TAttribute, bool> filter) where TAttribute : Attribute
        {
            var method = Assembly.GetEntryAssembly()
                .GetTypes()
                .Where(e => e.GetCustomAttribute<ViberControllerAttribute>() != null)
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

        public static object?[] MatchParameters(MethodInfo method, object context, params string?[] inputParameters)
        {
            var parameters = method.GetParameters();

            var requiredParametersCount = parameters.Count(e => !e.IsOptional);
            var parametersCount = parameters.Length;
            var inputParametersCount = inputParameters.Length;
            if (requiredParametersCount > inputParametersCount + 1 || inputParametersCount + 1 > parametersCount)
            {
                throw new InvalidOperationException();
            }

            var methodParams = new object?[parametersCount];
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
                else
                {
                    var localTargetParameterType = targetParameterType;

                    var isTargetParameterTypeNullable = targetParameterType.IsGenericType && targetParameterType.GetGenericTypeDefinition() == typeof(Nullable<>);
                    if (isTargetParameterTypeNullable)
                    {
                        var genericArgument = targetParameterType.GetGenericArguments()[0];
                        localTargetParameterType = genericArgument;
                    }

                    object? targetParameterValue = null;
                    if (!string.IsNullOrEmpty(sourceParameterValue))
                    {
                        if (localTargetParameterType == typeof(Guid))
                        {
                            if (Guid.TryParse(sourceParameterValue, out var guid))
                            {
                                targetParameterValue = guid;
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
                                targetParameterValue = Convert.ChangeType(sourceParameterValue, localTargetParameterType);
                            }
                            catch (InvalidCastException)
                            {
                                throw;
                            }
                        }
                    }
                    else if (!isTargetParameterTypeNullable)
                    {
                        throw new InvalidOperationException();
                    }

                    methodParams[j] = targetParameterValue;
                }
            }

            return methodParams;
        }
    }
}
