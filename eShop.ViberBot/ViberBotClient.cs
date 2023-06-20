using Newtonsoft.Json;
using System.Text;
using System.Threading;

namespace eShop.ViberBot
{
    public class ViberBotClient : IViberBotClient
    {
        private readonly ViberBotClientOptions _options;
        private readonly HttpClient _httpClient;

        public ViberBotClient(ViberBotClientOptions options, HttpClient httpClient)
        {
            _options = options;
            _httpClient = httpClient;

            _httpClient.BaseAddress = _options.Host;
            _httpClient.DefaultRequestHeaders.Add("X-Viber-Auth-Token", _options.Token);
        }

        public async Task SetWebhookAsync(string url, IEnumerable<EventType>? eventTypes = null, bool sendName = false, bool sendPhoto = false, CancellationToken cancellationToken = default)
        {
            var request = new SetWebhookRequest
            {
                Url = url,
                EventTypes = eventTypes,
                SendName = sendName,
                SendPhoto = sendPhoto,
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync("/pa/set_webhook", httpContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<SetWebhookResponse>(content);

            if (response.Status != 0)
            {
                throw new Exception(response.StatusMessage);
            }
        }

        public async Task SendPictureMessageAsync(string receiver, User sender, string media, string text, string? thumbnail = null, string? trackingData = null, string? minApiVersion = null, CancellationToken cancellationToken = default)
        {
            var message = new Message
            {
                Type = MessageType.Picture,
                Receiver = receiver,
                Sender = sender,
                TrackingData = trackingData,
                MinApiVersion = minApiVersion,
                Media = media,
                Text = text,
                Thumbnail = thumbnail,
            };

            var httpContent = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync("/pa/send_message", httpContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var response = JsonConvert.DeserializeObject<SendMessageResponse>(content);

            if (response.Status != 0)
            {
                throw new Exception(response.StatusMessage);
            }
        }
    }
}
