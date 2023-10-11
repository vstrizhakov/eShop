namespace eShop.Messaging
{
    internal interface IMessageObserver<TMessage> where TMessage : notnull, IMessage
    {
        event EventHandler<TMessage>? MessageReceived;
    }
}
