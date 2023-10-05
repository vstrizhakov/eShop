using eShop.Bots.Common;
using eShop.Telegram.Inner.Strategies;
using eShop.Telegram.Inner.Views;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Inner
{
    public class TelegramMiddleware : ITelegramMiddleware
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly IServiceProvider _serviceProvider;

        public TelegramMiddleware(IBotContextConverter botContextConverter, IServiceProvider serviceProvider)
        {
            _botContextConverter = botContextConverter;
            _serviceProvider = serviceProvider;
        }

        public async Task ProcessAsync(Update update, string? activeContext = null)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message!;
                var chat = message.Chat;
                if (chat.Type == ChatType.Private)
                {
                    if (message.Type == MessageType.Text)
                    {
                        var text = message.Text!;

                        var command = default(string);
                        var data = default(string);

                        if (text.StartsWith('/'))
                        {
                            command = text;

                            var seperatorIndex = text.IndexOf(' ');
                            if (seperatorIndex != -1)
                            {
                                command = text.Substring(0, seperatorIndex);
                                data = text.Substring(seperatorIndex + 1, text.Length - seperatorIndex - 1);
                            }
                        }
                        else if (activeContext != null)
                        {
                            data = activeContext;
                        }

                        await ProcessAsync(update, TelegramContext.TextMessage, command, data);
                    }
                    else if (message.Type == MessageType.Contact)
                    {
                        await ProcessAsync(update, TelegramContext.ContactMessage);
                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var callbackQuery = update.CallbackQuery!;
                var callbackData = callbackQuery.Data;

                await ProcessAsync(update, TelegramContext.CallbackQuery, data: callbackData);
            }
        }

        public async Task ProcessAsync(Update update, TelegramContext context, string? command = null, string? data = null)
        {
            var strategy = CreateStrategy(context, command, data);

            var method = strategy.PickControllerMethod();
            if (method != null)
            {
                if (method == null)
                {
                    throw new InvalidOperationException();
                }

                if (method.ReturnType != typeof(Task<ITelegramView?>))
                {
                    throw new InvalidOperationException();
                }

                var methodParams = strategy.GetParameters(method, update);

                var controller = method.DeclaringType;
                var obj = ActivatorUtilities.CreateInstance(_serviceProvider, controller);
                var task = (method.Invoke(obj, methodParams) as Task<ITelegramView?>)!;
                var view = await task;

                if (view != null)
                {
                    await view.ProcessAsync(
                        _serviceProvider.GetRequiredService<ITelegramBotClient>(),
                        _serviceProvider.GetRequiredService<IBotContextConverter>());
                }
            }
        }

        private IStrategy CreateStrategy(TelegramContext context, string? command, string? data)
        {
            var action = default(string);
            var parameters = default(string[]);

            if (data != null)
            {
                var arguments = _botContextConverter.Deserialize(data);
                if (arguments.Length > 0)
                {
                    action = arguments[0];
                    parameters = arguments.Skip(1).ToArray();
                }
            }

            return context switch
            {
                TelegramContext.CallbackQuery => new CallbackQueryStrategy(action, parameters),
                TelegramContext.TextMessage => new TextMessageStrategy(command, action, parameters),
                TelegramContext.ContactMessage => new ContactMessageStrategy(),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
