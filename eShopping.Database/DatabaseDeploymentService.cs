﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eShopping.Database
{
    internal class DatabaseDeploymentService<TContext> : IHostedService
        where TContext : DbContext
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseDeploymentService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<TContext>();

            await context.Database.EnsureCreatedAsync();

            var dataSeedings = scope.ServiceProvider.GetServices<IDataSeeding<TContext>>();
            foreach (var dataSeeding in dataSeedings)
            {
                await dataSeeding.SeedAsync(context);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}