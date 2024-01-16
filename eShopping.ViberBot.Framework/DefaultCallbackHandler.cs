using eShopping.Bots.Common;
using eShopping.ViberBot.Framework.Strategies;
using eShopping.ViberBot.Framework.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace eShopping.ViberBot.Framework
{
    internal class DefaultCallbackHandler : ICallbackHandler
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly IServiceProvider _serviceProvider;
        private readonly IViewRunner _viewRunner;
        private readonly IContextStore _contextStore;

        public DefaultCallbackHandler(
            IBotContextConverter botContextConverter,
            IServiceProvider serviceProvider,
            IViewRunner viewRunner,
            IContextStore contextStore)
        {
            _botContextConverter = botContextConverter;
            _serviceProvider = serviceProvider;
            _viewRunner = viewRunner;
            _contextStore = contextStore;
        }

        public async Task<Message?> HandleAsync(Callback callback)
        {
            var response = default(Message);

            if (callback.Event == EventType.ConversationStarted)
            {
                var data = callback.Context;

                response = await HandleAsync(callback, ViberContext.ConversationStarted, data);
            }
            else if (callback.Event == EventType.Message)
            {
                var message = callback.Message!;
                if (message.Type == MessageType.Text)
                {
                    var data = message.Text;

                    var activeContext = await _contextStore.GetActiveContextAsync(callback);

                    response = await HandleAsync(callback, ViberContext.TextMessage, data, activeContext);
                }
                else if (message.Type == MessageType.Contact)
                {
                    var activeContext = await _contextStore.GetActiveContextAsync(callback);

                    response = await HandleAsync(callback, ViberContext.ContactMessage, activeContext: activeContext);
                }
            }

            return response;
        }

        private async Task<Message?> HandleAsync(Callback callback, ViberContext context, string? data = null, string? activeContext = null)
        {
            var message = default(Message);

            var strategy = CreateStrategy(context, data, activeContext);

            var method = strategy.PickControllerMethod();
            if (method != null)
            {
                if (method == null)
                {
                    throw new InvalidOperationException();
                }

                if (method.ReturnType != typeof(Task<IViberView?>))
                {
                    throw new InvalidOperationException();
                }

                var methodParams = strategy.GetParameters(method, callback);

                var controller = method.DeclaringType;
                var obj = ActivatorUtilities.CreateInstance(_serviceProvider, controller);
                var task = (method.Invoke(obj, methodParams) as Task<IViberView?>)!;
                var view = await task;

                if (view != null)
                {
                    message = await _viewRunner.RunAsync(view);
                }
            }

            return message;
        }

        private IStrategy CreateStrategy(ViberContext context, string? data, string? activeContext)
        {
            var action = default(string);
            var parameters = Array.Empty<string>();

            if (data != null)
            {
                try
                {
                    var arguments = _botContextConverter.Deserialize(data);
                    if (arguments.Length > 0)
                    {
                        action = arguments[0];
                        parameters = arguments.Skip(1).ToArray();
                    }
                }
                catch
                {
                }
            }

            var activeAction = default(string);
            var activeParameters = Array.Empty<string>();
            if (activeContext != null)
            {
                var arguments = _botContextConverter.Deserialize(activeContext);
                if (arguments.Length > 0)
                {
                    activeAction = arguments[0];
                    activeParameters = arguments.Skip(1).ToArray();
                }
            }

            return context switch
            {
                ViberContext.ConversationStarted => new ConversationStartedStrategy(action, parameters),
                ViberContext.ContactMessage => new ContactMessageStrategy(action, parameters, activeAction, activeParameters),
                ViberContext.TextMessage => new TextMessageStrategy(action, parameters, activeAction, activeParameters),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
