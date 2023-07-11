using eShop.Distribution.DbContexts;
using eShop.Distribution.MessageHandlers;
using eShop.Distribution.Repositories;
using eShop.Messaging.Extensions;
using eShop.RabbitMq.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Converters;

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

            builder.Services.AddRabbitMqProducer();
            builder.Services.AddRabbitMqMessageHandler();

            builder.Services.AddScoped<TelegramUserCreateAccountResponseMessageHandler>();
            builder.Services.AddScoped<TelegramChatUpdatedEventHandler>();
            builder.Services.AddScoped<ViberUserCreateAccountUpdateMessageHandler>();
            builder.Services.AddScoped<ViberChatUpdatedEventHandler>();
            builder.Services.AddScoped<BroadcastCompositionMessageHandler>();
            builder.Services.AddScoped<BroadcastCompositionToTelegramUpdateEventHandler>();
            builder.Services.AddScoped<BroadcastCompositionToViberUpdateEventHandler>();

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