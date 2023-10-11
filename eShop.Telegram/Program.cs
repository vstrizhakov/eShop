using eShop.Bots.Common.Extensions;
using eShop.Common.Extensions;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging.Models.Distribution.ShopSettings;
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
            builder.Services.AddMessageHandler<TelegramUserCreateAccountResponseMessage, TelegramUserCreateAccountResponseMessageHandler>();
            builder.Services.AddMessageHandler<BroadcastCompositionToTelegramMessage, BroadcastCompositionToTelegramMessageHandler>();
            builder.Services.AddMessageHandler<GetPreferredCurrencyResponse, GetPreferredCurrencyResponseHandler>();
            builder.Services.AddMessageHandler<GetCurrenciesResponse, GetCurrenciesResponseHandler>();
            builder.Services.AddMessageHandler<SetPreferredCurrencyResponse, SetPreferredCurrencyResponseHandler>();
            builder.Services.AddMessageHandler<GetCurrencyRatesResponse, GetCurrencyRatesResponseHandler>();
            builder.Services.AddMessageHandler<GetCurrencyRateResponse, GetCurrencyRateResponseHandler>();
            builder.Services.AddMessageHandler<SetCurrencyRateResponse, SetCurrencyRateResponseHandler>();
            builder.Services.AddMessageHandler<GetComissionSettingsResponse, GetComissionSettingsResponseHandler>();
            builder.Services.AddMessageHandler<SetComissionShowResponse, SetComissionShowResponseHandler>();
            builder.Services.AddMessageHandler<GetComissionAmountResponse, GetComissionAmountResponseHandler>();
            builder.Services.AddMessageHandler<SetComissionAmountResponse, SetComissionAmountResponseHandler>();
            builder.Services.AddMessageHandler<GetShopSettingsResponse, GetShopSettingsResponseHandler>();
            builder.Services.AddMessageHandler<SetShopSettingsFilterResponse, SetShopSettingsFilterResponseHandler>();
            builder.Services.AddMessageHandler<GetShopSettingsShopsResponse, GetShopSettingsShopsResponseHandler>();
            builder.Services.AddMessageHandler<SetShopSettingsShopStateResponse, SetShopSettingsShopStateResponseHandler>();

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
                IdentityModelEventSource.ShowPII = true;

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