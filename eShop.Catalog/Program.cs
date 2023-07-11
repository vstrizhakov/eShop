using eShop.Catalog.DbContexts;
using eShop.Catalog.Repositories;
using eShop.Catalog.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using eShop.RabbitMq.Extensions;
using eShop.Common.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using eShop.Messaging.Extensions;
using eShop.Catalog.MessageHandlers;
using Newtonsoft.Json.Converters;

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

            builder.Services.AddDbContext<CatalogDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICompositionRepository, CompositionRepository>();

            builder.Services.AddScoped<IFileManager, FileManager>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:7000";
                    options.Audience = "api";
                });

            builder.Services.Configure<FilesConfiguration>(options => builder.Configuration.Bind("Files", options));

            builder.Services.AddRabbitMqProducer();
            builder.Services.AddRabbitMqMessageHandler();

            builder.Services.AddScoped<BroadcastCompositionUpdateEventHandler>();

            builder.Services.AddPublicUriBuilder(options => builder.Configuration.Bind("PublicUri", options));

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