namespace eShop.Messaging
{
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull, IResponse
    {
        Task<TResponse> HandleRequestAsync(TRequest request);
    }
}
