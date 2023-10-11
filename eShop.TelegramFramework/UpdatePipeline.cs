using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    internal class UpdatePipeline : IUpdatePipeline
    {
        private readonly IEnumerable<ITelegramMiddleware> _middlewares;

        public UpdatePipeline(IEnumerable<ITelegramMiddleware> middlewares)
        {
            _middlewares = middlewares;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            foreach (var middleware in _middlewares)
            {
                await middleware.HandleUpdateAsync(update);
            }
        }
    }
}
