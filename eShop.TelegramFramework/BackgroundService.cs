using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Types;

namespace eShop.TelegramFramework
{
    internal class BackgroundService : IHostedService
    {
        private readonly IUpdateObserver _updateObserver;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly object _tasksLock = new object();
        private readonly List<Task> _tasks = new List<Task>();

        public BackgroundService(IUpdateObserver updateObserver, IServiceScopeFactory serviceScopeFactory)
        {
            _updateObserver = updateObserver;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _updateObserver.UpdateArrived += UpdateObserver_UpdateArrived;
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _updateObserver.UpdateArrived -= UpdateObserver_UpdateArrived;

            await Task.WhenAll(_tasks);
        }

        private void AddTask(Task task)
        {
            lock (_tasksLock)
            {
                _tasks.Add(task);
            }
        }

        private void RemoveTask(Task task)
        {
            lock (_tasksLock)
            {
                _tasks.Remove(task);
            }
        }

        private void UpdateObserver_UpdateArrived(object? sender, Update update)
        {
            var task = Task.Run(async () =>
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var pipeline = scope.ServiceProvider.GetRequiredService<IUpdatePipeline>();
                await pipeline.HandleUpdateAsync(update);
            }).ContinueWith(task => RemoveTask(task));
            AddTask(task);
        }
    }
}
