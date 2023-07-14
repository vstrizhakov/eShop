using eShop.Common;
using eShop.Identity.DbContexts;
using eShop.Identity.Entities;
using eShop.Identity.MessageHandlers;
using eShop.Identity.Services;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            var publicUriSection = builder.Configuration.GetRequiredSection("PublicUri");
            var publicUriConfiguration = publicUriSection.Get<PublicUriConfiguration>();

            builder.Services
                .AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
                    options.UserInteraction.AllowOriginInReturnUrl = true;
                    options.UserInteraction.LoginUrl = new Uri(publicUriConfiguration.Host, "/auth/signIn").ToString();
                    options.UserInteraction.LogoutUrl = new Uri(publicUriConfiguration.Host, "/auth/signOut").ToString();
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryClients(Config.Clients(publicUriConfiguration.Host.OriginalString))
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService>();

            builder.Services.AddAuthentication();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<IdentityUserCreateAccountResponseMessage, IdentityUserCreateAccountResponseMessageHandler>();

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