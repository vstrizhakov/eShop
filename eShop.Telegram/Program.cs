using eShop.Telegram.Services;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using eShop.Bots.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using eShop.Telegram.DbContexts;
using eShop.Messaging.Extensions;
using eShop.Telegram.MessageHandlers;
using eShop.Telegram.Repositories;
using eShop.RabbitMq.Extensions;
using eShop.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

            builder.Services.AddDbContext<TelegramDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

            builder.Services.AddRabbitMqProducer();
            builder.Services.AddRabbitMqMessageHandler();

            builder.Services.AddScoped<TelegramUserCreateAccountResponseMessageHandler>();
            builder.Services.AddScoped<BroadcastCompositionToTelegramMessageHandler>();

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:7000";
                    options.Audience = "api";
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}