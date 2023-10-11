using eShop.Bots.Common.Extensions;
using eShop.Common.Extensions;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Viber.DbContexts;
using eShop.Viber.MessageHandlers;
using eShop.Viber.Repositories;
using eShop.Viber.Services;
using eShop.ViberBot;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace eShop.Viber
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

            builder.Services.AddDbContext<ViberDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));

            builder.Services.Configure<ViberBotConfiguration>(builder.Configuration.GetSection("ViberBot"));

            builder.Services.AddHttpClient("Viber")
                .AddTypedClient<IViberBotClient>((httpClient, serviceProvider) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IOptions<ViberBotConfiguration>>();
                    var options = new ViberBotClientOptions(configuration.Value.Token);
                    return new ViberBotClient(options, httpClient);
                });

            builder.Services.AddHostedService<ViberBotConfigurationService>();

            builder.Services.AddBotContextConverter();

            builder.Services.AddScoped<IViberUserRepository, ViberUserRepository>();

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<ViberUserCreateAccountUpdateMessage, ViberUserCreateAccountUpdateMessageHandler>();
            builder.Services.AddMessageHandler<BroadcastCompositionToViberMessage, BroadcastCompositionToViberMessageHandler>();

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";
                });

            builder.Services.AddScoped<IViberInvitationLinkGenerator, ViberInvitationLinkGenerator>();

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