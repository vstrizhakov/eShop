using eShop.Distribution.DbContexts;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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

            builder.Services.AddDbContext<DistributionDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IDistributionRepository, DistributionRepository>();

            builder.Services.AddScoped<IDistributionService, DistributionService>();
            builder.Services.AddScoped<IMessageBuilder, MessageBuilder>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddRabbitMq(options => options.HostName = "moonnightscout.pp.ua");
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<TelegramUserCreateAccountResponseMessage, TelegramUserCreateAccountResponseMessageHandler>();
            builder.Services.AddMessageHandler<TelegramChatUpdatedEvent, TelegramChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<ViberUserCreateAccountUpdateMessage, ViberUserCreateAccountUpdateMessageHandler>();
            builder.Services.AddMessageHandler<ViberChatUpdatedEvent, ViberChatUpdatedEventHandler>();
            builder.Services.AddMessageHandler<BroadcastCompositionMessage, BroadcastCompositionMessageHandler>();
            builder.Services.AddMessageHandler<BroadcastMessageUpdateEvent, BroadcastMessageUpdateEventHandler>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:7000";
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