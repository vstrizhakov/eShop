namespace eShop.Messaging
{
    internal interface IMessagePipeline<TMessage> where TMessage : IMessage
    {
        Task HandleMessageAsync(TMessage message);
    }
}
