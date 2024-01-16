namespace eShopping.Distribution.Services
{
    public class DefaultCurrencyRateSyncBackgroundService : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromDays(1);

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DefaultCurrencyRateSyncBackgroundService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var defaultCurrencyRateSyncService = scope.ServiceProvider.GetRequiredService<IDefaultCurrencyRateSyncService>();

                    await defaultCurrencyRateSyncService.SyncAsync();
                }
                catch (Exception ex)
                {
                    // TODO: Log error
                }

                await Task.Delay(Period, stoppingToken);
            }
        }
    }
}
