using AutoMapper;
using eShopping.Messaging.Contracts.Catalog;
using eShopping.Catalog.Repositories;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Catalog;
using MassTransit;

namespace eShopping.Catalog.Services
{
    public class SyncService : ISyncService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;
        private readonly IBus _producer;

        public SyncService(
            ICurrencyRepository currencyRepository,
            IShopRepository shopRepository,
            IMapper mapper,
            IBus producer)
        {
            _currencyRepository = currencyRepository;
            _shopRepository = shopRepository;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task SyncAsync()
        {
            var currencies = await _currencyRepository.GetCurrenciesAsync();
            if (currencies.Any())
            {
                var message = new SyncCurrenciesMessage
                {
                    Currencies = _mapper.Map<IEnumerable<Currency>>(currencies),
                };
                await _producer.Publish(message);
            }

            var shops = await _shopRepository.GetShopsAsync();
            if (shops.Any())
            {
                var message = new SyncShopsMessage
                {
                    Shops = _mapper.Map<IEnumerable<Shop>>(shops),
                };
                await _producer.Publish(message);
            }
        }
    }
}
