using eShopping.Bots.Common.Extensions;
using eShopping.Common.Extensions;
using eShopping.Messaging;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Telegram.Bot;
using eShopping.Database.Extensions;
using eShopping.Telegram.Services;
using eShopping.Telegram.Repositories;
using eShopping.Telegram.TelegramFramework.Middlewares;
using eShopping.Telegram.DbContexts;
using eShopping.Telegram.TelegramFramework;

namespace eShopping.Telegram
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
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsDevelopment())
            {
                var executionRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, "appsettings.json"), true, true);
                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, $"appsettings.{builder.Environment.EnvironmentName}.json"), true, true);
            }

            builder.Services.AddDbContext<TelegramDbContext>(options =>
                options.UseCosmos(builder.Configuration.GetConnectionString("Default"), "eShop"));

            builder.Services.AddDatabaseDeployment<TelegramDbContext>();

            builder.Services.Configure<TelegramBotConfiguration>(builder.Configuration.GetSection("TelegramBot"));

            builder.Services.AddHttpClient("Telegram")
                .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                {
                    var botConfiguration = serviceProvider.GetRequiredService<IOptions<TelegramBotConfiguration>>();
                    var options = new TelegramBotClientOptions(botConfiguration.Value.Token);
                    var botClient = new TelegramBotClient(options, httpClient);

                    var logger = serviceProvider.GetRequiredService<ILogger<TelegramBotClient>>();
                    botClient.OnMakingApiRequest += async (botClient, args, cancellationToken) =>
                    {
                        logger.LogInformation($"[{DateTime.UtcNow.TimeOfDay}] Sending API reguest...");
                    };

                    return botClient;
                });

            builder.Services.AddHostedService<TelegramBotConfigurationService>();

            builder.Services.AddBotContextConverter();

            builder.Services.AddScoped<ITelegramUserRepository, TelegramUserRepository>();
            builder.Services.AddScoped<ITelegramChatRepository, TelegramChatRepository>();

            builder.Services.Configure<AzureServiceBusOptions>(options => builder.Configuration.Bind("AzureServiceBus", options));
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<AzureServiceBusOptions>>().Value;
                    cfg.Host(options.ConnectionString);

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";
                });

            builder.Services.AddScoped<ITelegramService, TelegramService>();

            builder.Services.AddScoped<ITelegramMiddleware, IdentityManagementTelegramMiddleware>();
            builder.Services.AddTelegramFramework<TelegramContextStore>();

            builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
            builder.Services.AddScoped<IRateLimitedTelegramBotClient, RateLimitedTelegramBotClient>();

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