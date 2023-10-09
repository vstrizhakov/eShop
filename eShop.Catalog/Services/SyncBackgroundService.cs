namespace eShop.Catalog.Services
{
    public class SyncBackgroundService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SyncBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var currencyService = scope.ServiceProvider.GetRequiredService<ISyncService>();
            await currencyService.SyncAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
