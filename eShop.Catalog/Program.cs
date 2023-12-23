using eShop.Catalog.DbContexts;
using eShop.Catalog.Hubs;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common.Extensions;
using eShop.Database.Extensions;
using eShop.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace eShop.Catalog
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
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            if (builder.Environment.IsDevelopment())
            {
                var executionRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, "appsettings.json"), true, true);
                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, $"appsettings.{builder.Environment.EnvironmentName}.json"), true, true);
            }

            builder.Services.AddDbContext<CatalogDbContext>(options =>
                options.UseCosmos(builder.Configuration.GetConnectionString("Default"), "eShop"));

            builder.Services.AddDatabaseDeployment<CatalogDbContext>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<IAnnounceRepository, AnnounceRepository>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();

            builder.Services.AddSingleton<IFileManager, AzureBlobManager>();
            //builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<IAnnouncesService, AnnounceService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<ISyncService, SyncService>();
            builder.Services.AddScoped<IAnnouncesHubServer, AnnounceHubServer>();
            builder.Services.AddScoped<ICurrencyService, CurrencyService>();

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
                                (path.StartsWithSegments("/api/catalog/ws")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

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

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));

            builder.Services.AddHostedService<SyncBackgroundService>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //var filesConfiguration = app.Services.GetRequiredService<IOptions<FilesConfiguration>>();
            //var staticFilesDirectory = Path.Combine(filesConfiguration.Value.Root, "Catalog");
            //if (!Directory.Exists(staticFilesDirectory))
            //{
            //    Directory.CreateDirectory(staticFilesDirectory);
            //}

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(staticFilesDirectory),
            //    RequestPath = "/catalog",
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<AnnouncesHub>("/api/catalog/ws");

            app.Run();
        }
    }
}