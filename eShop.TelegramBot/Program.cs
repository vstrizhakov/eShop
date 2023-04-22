using eShop.TelegramBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace eShop.TelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var botConfigurationSection = builder.Configuration.GetSection(nameof(BotConfiguration));
            builder.Services.Configure<BotConfiguration>(botConfigurationSection);

            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            builder.Services.AddHttpClient("Telegram")
                .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                {
                    var botConfiguration = serviceProvider.GetRequiredService<IOptions<BotConfiguration>>();
                    var options = new TelegramBotClientOptions(botConfiguration.Value.BotToken);
                    return new TelegramBotClient(options, httpClient);
                });

            builder.Services.AddHostedService<ConfigureWebhookService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpLogging();

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}