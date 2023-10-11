namespace eShop.Messaging
{
    internal interface IMessagePublisher<TMessage> where TMessage : notnull, IMessage
    {
        void Publish(TMessage message);
    }
}
