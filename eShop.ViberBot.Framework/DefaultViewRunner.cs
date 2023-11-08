using eShop.Bots.Common;

namespace eShop.ViberBot.Framework
{
    internal class DefaultViewRunner : IViewRunner
    {
        private readonly IBotContextConverter _botContextConverter;
        private readonly IViberBotClient _botClient;

        public DefaultViewRunner(IBotContextConverter botContextConverter, IViberBotClient botClient)
        {
            _botContextConverter = botContextConverter;
            _botClient = botClient;
        }

        public async Task<Message?> RunAsync(IViberView view)
        {
            var message = await RunAsync(view, null);
            return message;
        }

        public async Task<Message?> RunAsync(IViberView view, ViberContext? context)
        {
            var message = view.Build(_botContextConverter);
            if (context != ViberContext.ConversationStarted)
            {
                await _botClient.SendMessageAsync(message);

                message = null;
            }

            return message;
        }
    }
}
