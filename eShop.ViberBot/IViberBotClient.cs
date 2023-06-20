namespace eShop.ViberBot
{
    public interface IViberBotClient
    {
        Task SetWebhookAsync(string url, IEnumerable<EventType>? eventTypes = null, bool sendName = false, bool sendPhoto = false, CancellationToken cancellationToken = default);
        Task SendPictureMessageAsync(string receiver, User sender, string media, string text, string? thumbnail = null, string? trackingData = null, string? minApiVersion = null, CancellationToken cancellationToken = default);
    }
}
