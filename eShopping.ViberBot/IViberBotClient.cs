namespace eShopping.ViberBot
{
    public interface IViberBotClient
    {
        Task SetWebhookAsync(string url, IEnumerable<EventType>? eventTypes = null, bool sendName = false, bool sendPhoto = false, CancellationToken cancellationToken = default);
        Task SendPictureMessageAsync(string receiver, User sender, string media, string text, string? thumbnail = null, string? trackingData = null, int? minApiVersion = null, Keyboard? keyboard = null, CancellationToken cancellationToken = default);
        Task SendTextMessageAsync(string receiver, User sender, string text, string? trackingData = null, int? minApiVersion = null, Keyboard? keyboard = null, CancellationToken cancellationToken = default);
        Task SendMessageAsync(Message message, CancellationToken cancellationToken = default);
    }
}
