using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace eShopping.Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", false, true);

            builder.Services.AddOcelot();
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseWebSockets();

            app.Use(async (context, next) =>
            {
                await next();
            });

            await app.UseOcelot();

            await app.RunAsync();
        }
    }
}