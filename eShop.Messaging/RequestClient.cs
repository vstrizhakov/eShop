using Microsoft.Extensions.DependencyInjection;

namespace eShop.Messaging
{
    internal class RequestClient : IRequestClient
    {
        private readonly IProducer _producer;
        private readonly IServiceProvider _serviceProvider;

        public RequestClient(IProducer producer, IServiceProvider serviceProvider)
        {
            _producer = producer;
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
            where TResponse : notnull, IResponse
        {
            var observer = _serviceProvider.GetRequiredService<IMessageObserver<TResponse>>();

            var taskCompletionSource = new TaskCompletionSource<TResponse>();

            void OnMessageReceived(object? sender, TResponse response)
            {
                taskCompletionSource.TrySetResult(response);
            }

            observer.MessageReceived += OnMessageReceived;

            _producer.Publish(request);

            var response = await taskCompletionSource.Task;

            observer.MessageReceived -= OnMessageReceived;

            return response;
        }
    }
}
