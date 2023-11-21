using eShop.Catalog.DbContexts;
using eShop.Catalog.Handlers;
using eShop.Catalog.Hubs;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common.Extensions;
using eShop.Configuration;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
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

                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, $"appsettings.{builder.Environment.EnvironmentName}.json"), true, true);
                builder.Configuration.AddJsonFile(Path.Combine(executionRoot, "appsettings.json"), true, true);
            }

            builder.Services.AddDbContext<CatalogDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IAnnounceRepository, AnnounceRepository>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();

            builder.Services.AddSingleton<IFileManager, AzureBlobManager>();
            //builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<IAnnouncesService, AnnounceService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<ISyncService, SyncService>();
            builder.Services.AddScoped<IAnnouncesHubServer, AnnounceHubServer>();

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

            builder.Services.Configure<FilesConfiguration>(options => builder.Configuration.Bind("Files", options));

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<BroadcastAnnounceUpdateEvent, BroadcastAnnounceUpdateEventHandler>();
            builder.Services.AddRequestHandler<GetCurrenciesRequest, GetCurrenciesResponse, GetCurrenciesRequestHandler>();


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