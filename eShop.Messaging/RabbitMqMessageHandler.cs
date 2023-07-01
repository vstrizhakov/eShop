using eShop.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;

namespace eShop.Messaging
{
    internal class RabbitMqMessageHandler : IRabbitMqMessageHandler
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqMessageHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleMessageAsync(string message)
        {
            var internalMessage = JsonConvert.DeserializeObject<Message>(message);
            var typeName = internalMessage.Name;
            
            using var scope = _scopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes());
            var messageHandler = allTypes
                .FirstOrDefault(e => e.Name.Contains(typeName));

            if (messageHandler != null)
            {
                var service = serviceProvider.GetService(messageHandler);
                if (service != null)
                {
                    var method = messageHandler.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public)
                        .FirstOrDefault(e => e.Name == "HandleMessageAsync");
                    var type = allTypes.FirstOrDefault(e => e.Name == typeName);
                    if (method != null && type != null)
                    {
                        if (internalMessage.TryGetData(type, out var messageData))
                        {
                            var result = method.Invoke(service, new[] { messageData });
                            if (result is Task task)
                            {
                                await task;
                            }
                        }
                    }
                }
            }
        }
    }
}
