using eShop.Distribution.DbContexts;
using eShop.Distribution.Handlers;
using eShop.Distribution.Hubs;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging.Models.Distribution.ShopSettings;
using eShop.Messaging.Models.Telegram;
using eShop.Messaging.Models.Viber;
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

            builder.Services.AddSignalR()
                .AddNewtonsoftJsonProtocol(options =>
                {
                    options.PayloadSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
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
            builder.Services.AddScoped<IDistributionsHubServer, DistributionsHubServer>();

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<AccountRegisteredEvent, AccountRegisteredEventHandler>();
            builder.Services.AddMessageHandler<AccountUpdatedEvent, AccountUpdatedEventHandler>();
            builder.Services.AddMessageHandler<TelegramChatUpdatedEvent, TelegramChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<ViberChatUpdatedEvent, ViberChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<BroadcastAnnounceMessage, BroadcastCompositionMessageHandler>();
            builder.Services.AddMessageHandler<BroadcastMessageUpdateEvent, BroadcastMessageUpdateEventHandler>();
            builder.Services.AddMessageHandler<SyncCurrenciesMessage, SyncCurrenciesMessageHandler>();
            builder.Services.AddRequestHandler<GetPreferredCurrencyRequest, GetPreferredCurrencyResponse, GetPreferredCurrencyRequestHandler>();
            builder.Services.AddRequestHandler<SetPreferredCurrencyRequest, SetPreferredCurrencyResponse, SetPreferredCurrencyRequestHandler>();
            builder.Services.AddRequestHandler<GetCurrencyRatesRequest, GetCurrencyRatesResponse, GetCurrencyRatesRequestHandler>();
            builder.Services.AddRequestHandler<SetCurrencyRateRequest, SetCurrencyRateResponse, SetCurrencyRateRequestHandler>();
            builder.Services.AddRequestHandler<GetCurrencyRateRequest, GetCurrencyRateResponse, GetCurrencyRateRequestHandler>();
            builder.Services.AddRequestHandler<GetComissionSettingsRequest, GetComissionSettingsResponse, GetComissionSettingsRequestHandler>();
            builder.Services.AddRequestHandler<SetComissionAmountRequest, SetComissionAmountResponse, SetComissionAmountRequestHandler>();
            builder.Services.AddRequestHandler<GetComissionAmountRequest, GetComissionAmountResponse, GetComissionAmountRequestHandler>();
            builder.Services.AddRequestHandler<GetShopSettingsRequest, GetShopSettingsResponse, GetShopSettingsRequestHandler>();
            builder.Services.AddRequestHandler<SetShopSettingsFilterRequest, SetShopSettingsFilterResponse, SetShopSettingsFilterRequestHandler>();
            builder.Services.AddRequestHandler<GetShopSettingsShopsRequest, GetShopSettingsShopsResponse, GetShopSettingsShopsRequestHandler>();
            builder.Services.AddRequestHandler<SetShopSettingsShopStateRequest, SetShopSettingsShopStateResponse, SetShopSettingsShopStateRequestHandler>();
            builder.Services.AddRequestHandler<GetDistributionSettingsRequest, GetDistributionSettingsResponse, GetDistributionSettingsRequestHandler>();
            builder.Services.AddRequestHandler<SetShowSalesRequest, SetShowSalesResponse, SetShowSalesRequestHandler>();

            builder.Services.AddMessageHandler<SyncShopsMessage, SyncShopsMessageHandler>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/api/distribution/ws")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
            app.MapHub<DistributionsHub>("/api/distribution/ws");

            app.Run();
        }
    }
}