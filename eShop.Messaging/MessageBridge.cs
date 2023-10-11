namespace eShop.Messaging
{
    internal class MessageBridge<TMessage> : IMessagePublisher<TMessage>, IMessageObserver<TMessage>
        where TMessage : notnull, IMessage
    {
        public event EventHandler<TMessage>? MessageReceived;

        public void Publish(TMessage message)
        {
            MessageReceived?.Invoke(this, message);
        }
    }
}
