using eShop.Database.Data;
using eShop.PromUaApi;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace eShop.Services
{
    public class PromService : BackgroundService
    {
        private const string Token = "b6824dbfefc7f94ee82aae5eff013486477f0df8";
        private const int Limit = 1000;
        private const int CompositionCapacity = 1;

        private readonly ITelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;

        public PromService(ITelegramBotClient botClient, IServiceScopeFactory scopeFactory)
        {
            _botClient = botClient;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://my.prom.ua"),
            };

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            var products = new List<PromUaApi.Product>();

            var @continue = true;
            long? lastId = null;
            do
            {
                var requestUrl = QueryHelpers.AddQueryString("/api/v1/products/list", "limit", Limit.ToString());
                if (lastId != null)
                {
                    requestUrl = QueryHelpers.AddQueryString(requestUrl, "last_id", lastId.ToString());
                }
                var httpResponse = await httpClient.GetAsync(requestUrl);
                httpResponse.EnsureSuccessStatusCode();
                var content = await httpResponse.Content.ReadAsStringAsync();

                var response = JsonConvert.DeserializeObject<GetProductsListResponse>(content);
                @continue = response != null;

                if (response != null)
                {
                    products.AddRange(response.Products);

                    @continue = response.Products.Any();

                    var lastProduct = response.Products.LastOrDefault();
                    if (lastProduct != null)
                    {
                        lastId = lastProduct.Id;
                    }
                }
            } while (@continue && products.Count < 100);

            Debug.WriteLine($"Total products amount: {products.Count}");

            var composition = new List<PromUaApi.Product>();

            var random = new Random();
            var i = 0;
            while (i++ < CompositionCapacity)
            {
                var index = random.Next(products.Count);
                var product = products[index];

                composition.Add(product);

                products.RemoveAt(index);
            }

            var media = composition.Select(e => new InputMediaPhoto(new InputFileUrl(e.MainImage))).ToList();
            var firstMedia = media.FirstOrDefault();
            firstMedia.Caption = products[0].Name;

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var telegramChat = dbContext.TelegramChats.FirstOrDefault();

            await _botClient.SendMediaGroupAsync(new ChatId(telegramChat.ExternalId), media);
        }
    }
}
