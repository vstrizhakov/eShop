namespace eShop.Catalog.Services
{
    public class CurrenciesSyncService : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CurrenciesSyncService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var currencyService = scope.ServiceProvider.GetRequiredService<ICurrencyService>();
            await currencyService.SyncCurrenciesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
