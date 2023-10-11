namespace eShop.Messaging
{
    internal class RequestHandler<TRequest, TResponse> : IMessageHandler<TRequest>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull, IResponse
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IProducer _producer;

        public RequestHandler(IRequestHandler<TRequest, TResponse> requestHandler, IProducer producer)
        {
            _requestHandler = requestHandler;
            _producer = producer;
        }

        public async Task HandleMessageAsync(TRequest request)
        {
            var response = await _requestHandler.HandleRequestAsync(request);

            response.RequestId = request.RequestId; // TODO: seems hukky-hackky

            _producer.Publish(response);
        }
    }
}
