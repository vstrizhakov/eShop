using eShop.Catalog.DbContexts;
using eShop.Catalog.MessageHandlers;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using eShop.Common.Extensions;
using eShop.Configuration;
using eShop.Messaging.Extensions;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

            builder.Services.AddDbContext<CatalogDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString(Assembly.GetExecutingAssembly().GetName().Name)));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICompositionRepository, CompositionRepository>();
            builder.Services.AddScoped<IShopRepository, ShopRepository>();

            builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<ICompositionService, CompositionService>();
            builder.Services.AddScoped<IShopService, ShopService>();
            builder.Services.AddScoped<ICurrencyService, CurrencyService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration["PublicUri:Identity"];
                    options.Audience = "api";
                });

            builder.Services.Configure<FilesConfiguration>(options => builder.Configuration.Bind("Files", options));

            builder.Services.AddRabbitMq(options => builder.Configuration.Bind("RabbitMq", options));
            builder.Services.AddRabbitMqProducer();
            builder.Services.AddMessageHandler<BroadcastCompositionUpdateEvent, BroadcastCompositionUpdateEventHandler>();
            builder.Services.AddMessageHandler<GetCurrenciesRequest, GetCurrenciesRequestHandler>();

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));

            builder.Services.AddHostedService<CurrenciesSyncService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            var filesConfiguration = app.Services.GetRequiredService<IOptions<FilesConfiguration>>();
            var staticFilesDirectory = Path.Combine(filesConfiguration.Value.Root, "Catalog");
            if (!Directory.Exists(staticFilesDirectory))
            {
                Directory.CreateDirectory(staticFilesDirectory);
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(staticFilesDirectory),
                RequestPath = "/catalog",
            });

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}