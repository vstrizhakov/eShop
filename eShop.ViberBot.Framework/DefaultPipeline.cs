namespace eShop.ViberBot.Framework
{
    internal class DefaultPipeline : IPipeline
    {
        private readonly IEnumerable<IViberMiddleware> _middlewares;
        private readonly ICallbackHandler _callbackHandler;

        public DefaultPipeline(IEnumerable<IViberMiddleware> middlewares, ICallbackHandler callbackHandler)
        {
            _middlewares = middlewares;
            _callbackHandler = callbackHandler;
        }

        public async Task<Message?> HandleAsync(Callback callback)
        {
            foreach (var middleware in _middlewares)
            {
                await middleware.ProcessAsync(callback);
            }

            var response = await _callbackHandler.HandleAsync(callback);
            return response;
        }
    }
}
