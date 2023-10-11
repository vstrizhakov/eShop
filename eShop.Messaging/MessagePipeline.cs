using Microsoft.Extensions.DependencyInjection;

namespace eShop.Messaging
{
    internal class MessagePipeline<TMessage> : IMessagePipeline<TMessage>
        where TMessage : IMessage
    {
        private readonly IMessagePublisher<TMessage> _publisher;
        private readonly IServiceProvider _serviceProvider;

        public MessagePipeline(IMessagePublisher<TMessage> publisher, IServiceProvider serviceProvider)
        {
            _publisher = publisher;
            _serviceProvider = serviceProvider;
        }

        public async Task HandleMessageAsync(TMessage message)
        {
            _publisher.Publish(message);

            var handler = _serviceProvider.GetService<IMessageHandler<TMessage>>();
            if (handler != null)
            {
                await handler.HandleMessageAsync(message);
            }
        }
    }
}
