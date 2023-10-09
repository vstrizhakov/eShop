namespace eShop.Messaging
{
    public interface IRequest<TResponse> : IMessage where TResponse : IResponse
    {
    }
}
