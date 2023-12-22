using eShop.Bots.Links;
using eShop.Common.Extensions;
using eShop.Identity.DbContexts;
using eShop.Identity.Entities;
using eShop.Identity.Repositories;
using eShop.Identity.Services;
using eShop.Messaging;
using MassTransit;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using eShop.Database.Extensions;
using eShop.Identity.Stores;

namespace eShop.Identity
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            builder.Services.AddDbContext<IdentityDbContext>(options =>
                options.UseCosmos(builder.Configuration.GetConnectionString("Default"), "eShop"));

            builder.Services.AddDatabaseDeployment<IdentityDbContext>();

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>();

            var host = new Uri(builder.Configuration["PublicUri:Host"]);

            builder.Services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
                    options.UserInteraction.AllowOriginInReturnUrl = true;
                    options.UserInteraction.LoginUrl = new Uri(host, "/auth/signIn").ToString();
                    options.UserInteraction.LogoutUrl = new Uri(host, "/auth/signOut").ToString();
                    options.UserInteraction.ErrorUrl = new Uri(host, "/auth/error").ToString();
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients(host.OriginalString))
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();

            builder.Services.AddAuthentication()
                .AddCookie("PhoneNumberConfirmationCookie");

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddTelegramLinks(options =>
            {
                options.Username = builder.Configuration["TelegramBot:Username"];
            });

            builder.Services.AddViberLinks(options =>
            {
                options.ChatUrl = builder.Configuration["ViberBot:ChatUrl"];
            });

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseForwardedHeaders();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}