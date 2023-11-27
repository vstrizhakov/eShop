using eShop.Bots.Common.Extensions;
using eShop.Common.Extensions;
using eShop.Messaging;
using eShop.Telegram.DbContexts;
using eShop.Telegram.Repositories;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework;
using eShop.Telegram.TelegramFramework.Middlewares;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Extensions;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
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

            builder.Services.Configure<RabbitMqOptions>(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());

                x.SetKebabCaseEndpointNameFormatter();

                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
                    cfg.Host(options.HostName);

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