namespace eShop.Messaging
{
    public interface IRequestClient
    {
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
            where TResponse : notnull, IResponse;
    }
}
