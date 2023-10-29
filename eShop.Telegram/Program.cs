using eShop.Bots.Common.Extensions;
using eShop.Common.Extensions;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Messaging.Models.Telegram;
using eShop.Telegram.DbContexts;
using eShop.Telegram.MessageHandlers;
using eShop.Telegram.Repositories;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework;
using eShop.Telegram.TelegramFramework.Middlewares;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Telegram.Bot;

namespace eShop.Telegram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsDevelopment())
            {
                var executionRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, $"appsettings.{builder.Environment.EnvironmentName}.json"), true, true);
                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, "appsettings.json"), true, true);
            }

            builder.Services.AddDbContext<TelegramDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));

            builder.Services.Configure<TelegramBotConfiguration>(builder.Configuration.GetSection("TelegramBot"));

            builder.Services.AddHttpClient("Telegram")
                .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                {
                    var botConfiguration = serviceProvider.GetRequiredService<IOptions<TelegramBotConfiguration>>();
                    var options = new TelegramBotClientOptions(botConfiguration.Value.Token);
                    return new TelegramBotClient(options, httpClient);
                });

            builder.Services.AddHostedService<TelegramBotConfigurationService>();

            builder.Services.AddBotContextConverter();

            builder.Services.AddScoped<ITelegramUserRepository, TelegramUserRepository>();
            builder.Services.AddScoped<ITelegramChatRepository, TelegramChatRepository>();

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();

            builder.Services.AddMessageHandler<BroadcastCompositionToTelegramMessage, BroadcastCompositionToTelegramMessageHandler>();
            builder.Services.AddMessageListener<RegisterTelegramUserResponse>();
            builder.Services.AddMessageListener<GetPreferredCurrencyResponse>();
            builder.Services.AddMessageListener<GetCurrenciesResponse>();
            builder.Services.AddMessageListener<SetPreferredCurrencyResponse>();
            builder.Services.AddMessageListener<GetCurrencyRatesResponse>();
            builder.Services.AddMessageListener<GetCurrencyRateResponse>();
            builder.Services.AddMessageListener<SetCurrencyRateResponse>();
            builder.Services.AddMessageListener<GetComissionSettingsResponse>();
            builder.Services.AddMessageListener<GetComissionAmountResponse>();
            builder.Services.AddMessageListener<SetComissionAmountResponse>();
            builder.Services.AddMessageListener<GetShopSettingsResponse>();
            builder.Services.AddMessageListener<SetShopSettingsFilterResponse>();
            builder.Services.AddMessageListener<GetShopSettingsShopsResponse>();
            builder.Services.AddMessageListener<SetShopSettingsShopStateResponse>();
            builder.Services.AddMessageListener<SetShowSalesResponse>();
            builder.Services.AddMessageListener<GetDistributionSettingsResponse>();

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";
                });

            builder.Services.AddScoped<ITelegramInvitationLinkGenerator, TelegramInvitationLinkGenerator>();
            builder.Services.AddScoped<ITelegramService, TelegramService>();

            builder.Services.AddScoped<ITelegramMiddleware, IdentityManagementTelegramMiddleware>();
            builder.Services.AddTelegramFramework<TelegramContextStore>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}