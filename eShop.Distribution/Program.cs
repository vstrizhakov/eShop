using eShop.Distribution.DbContexts;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging.Models.Distribution.ShopSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace eShop.Distribution
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

            builder.Services.AddDbContext<DistributionDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));
            
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IDistributionRepository, DistributionRepository>();
            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<IDistributionSettingsRepository, DistributionSettingsRepository>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();

            builder.Services.AddScoped<IDistributionService, DistributionService>();
            builder.Services.AddScoped<IMessageBuilder, MessageBuilder>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ICurrencyService, CurrencyService>();
            builder.Services.AddScoped<IDistributionSettingsService, DistributionSettingsService>();
            builder.Services.AddScoped<IShopService, ShopService>();

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<TelegramUserCreateAccountResponseMessage, TelegramUserCreateAccountResponseMessageHandler>();
            builder.Services.AddMessageHandler<TelegramChatUpdatedEvent, TelegramChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<ViberUserCreateAccountUpdateMessage, ViberUserCreateAccountUpdateMessageHandler>();
            builder.Services.AddMessageHandler<ViberChatUpdatedEvent, ViberChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<BroadcastCompositionMessage, BroadcastCompositionMessageHandler>();
            builder.Services.AddMessageHandler<BroadcastMessageUpdateEvent, BroadcastMessageUpdateEventHandler>();
            builder.Services.AddMessageHandler<SyncCurrenciesMessage, SyncCurrenciesMessageHandler>();
            builder.Services.AddMessageHandler<GetPreferredCurrencyRequest, GetPreferredCurrencyRequestHandler>();
            builder.Services.AddMessageHandler<SetPreferredCurrencyRequest, SetPreferredCurrencyRequestHandler>();
            builder.Services.AddMessageHandler<GetCurrencyRatesRequest, GetCurrencyRatesRequestHandler>();
            builder.Services.AddMessageHandler<SetCurrencyRateRequest, SetCurrencyRateRequestHandler>();
            builder.Services.AddMessageHandler<GetCurrencyRateRequest, GetCurrencyRateRequestHandler>();
            builder.Services.AddMessageHandler<GetComissionSettingsRequest, GetComissionSettingsRequestHandler>();
            builder.Services.AddMessageHandler<SetComissionShowRequest, SetComissionShowRequestHandler>();
            builder.Services.AddMessageHandler<SetComissionAmountRequest, SetComissionAmountRequestHandler>();
            builder.Services.AddMessageHandler<GetComissionAmountRequest, GetComissionAmountRequestHandler>();
            builder.Services.AddMessageHandler<SyncShopsMessage, SyncShopsMessageHandler>();
            builder.Services.AddMessageHandler<GetShopSettingsRequest, GetShopSettingsRequestHandler>();
            builder.Services.AddMessageHandler<SetShopSettingsFilterRequest, SetShopSettingsFilterRequestHandler>();
            builder.Services.AddMessageHandler<GetShopSettingsShopsRequest, GetShopSettingsShopsRequestHandler>();
            builder.Services.AddMessageHandler<SetShopSettingsShopStateRequest, SetShopSettingsShopStateRequestHandler>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";
                });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}