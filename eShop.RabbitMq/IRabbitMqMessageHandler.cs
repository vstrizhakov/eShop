namespace eShop.RabbitMq
{
    public interface IRabbitMqMessageHandler
    {
        Task HandleMessageAsync(string message);
    }
}
