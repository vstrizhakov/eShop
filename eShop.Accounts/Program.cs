using eShop.Accounts.DbContexts;
using eShop.Accounts.MessageHandlers;
using eShop.Accounts.Repositories;
using eShop.Accounts.Services;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace eShop.Accounts
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

            builder.Services.AddRabbitMq(options => options.HostName = "moonnightscout.pp.ua");
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<TelegramUserCreateAccountRequestMessage, TelegramUserCreateAccountRequestMessageHandler>();
            builder.Services.AddMessageHandler<ViberUserCreateAccountRequestMessage, ViberUserCreateAccountRequestMessageHandler>();
            builder.Services.AddMessageHandler<IdentityUserCreateAccountRequestMessage, IdentityUserCreateAccountRequestMessageHandler>();

            if (builder.Environment.IsDevelopment())
            {
                var executionRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, $"appsettings.{builder.Environment.EnvironmentName}.json"), true, true);
                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, "appsettings.json"), true, true);
            }

            builder.Services.AddDbContext<AccountsDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:7000";
                    options.Audience = "api";
                });

            builder.Services.AddScoped<IAccountService, AccountService>();

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