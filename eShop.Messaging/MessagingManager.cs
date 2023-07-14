using Microsoft.Extensions.Hosting;

namespace eShop.Messaging
{
    internal class MessagingManager : IHostedService
    {
        private readonly IEnumerable<IConsumer> _consumers;

        public MessagingManager(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Start();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Stop();
            }
            
            return Task.CompletedTask;
        }
    }
}
