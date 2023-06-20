using eShop.Configurations;
using eShop.Database.Data;
using eShop.Services;
using eShop.ViberBot;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Telegram.Bot;

namespace eShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var botConfigurationSection = builder.Configuration.GetSection("TelegramBot");
            builder.Services.Configure<TelegramBotConfiguration>(botConfigurationSection);
            builder.Services.Configure<ViberBotConfiguration>(builder.Configuration.GetSection("ViberBot"));
            builder.Services.Configure<ApplicationConfiguration>(builder.Configuration.GetSection("Application"));

            var mvcBuilder = builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson();

            if (builder.Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            builder.Services.AddHttpClient("Telegram")
                .AddTypedClient<ITelegramBotClient>((httpClient, serviceProvider) =>
                {
                    var botConfiguration = serviceProvider.GetRequiredService<IOptions<TelegramBotConfiguration>>();
                    var options = new TelegramBotClientOptions(botConfiguration.Value.Token);
                    return new TelegramBotClient(options, httpClient);
                });

            builder.Services.AddHttpClient("Viber")
                .AddTypedClient<IViberBotClient>((httpClient, serviceProvider) =>
                {
                    var configuration = serviceProvider.GetRequiredService<IOptions<ViberBotConfiguration>>();
                    var options = new ViberBotClientOptions(configuration.Value.Token);
                    return new ViberBotClient(options, httpClient);
                });

            builder.Services.AddHostedService<TelegramBotConfigurationService>();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
                innerOptions => innerOptions.MigrationsAssembly("eShop.Database")));

            builder.Services.AddIdentity<Database.Data.User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddScoped<IFileManager, FileManager>();
            //builder.Services.AddHostedService<PromService>();

            builder.Services.AddTransient<IPublicUriBuilder, PublicUriBuilder>();
            builder.Services.AddHostedService<ViberBotConfigurationService>();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";

                options.DefaultAuthenticateScheme = null;
                options.DefaultSignInScheme = null;
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://localhost:5001";

                    options.ClientId = "main";
                    options.ClientSecret = "8E643505-E1B5-4F8A-8120-E84D50E1CAF6";
                    options.ResponseType = "code";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");

                    options.SaveTokens = true;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();

            await app.RunAsync();
        }
    }
}