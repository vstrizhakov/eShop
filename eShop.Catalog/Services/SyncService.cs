using AutoMapper;
using eShop.Catalog.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;

namespace eShop.Catalog.Services
{
    public class SyncService : ISyncService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public SyncService(
            ICurrencyRepository currencyRepository,
            IShopRepository shopRepository,
            IMapper mapper,
            IProducer producer)
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
                _producer.Publish(message);
            }

            var shops = await _shopRepository.GetShopsAsync();
            if (shops.Any())
            {
                var message = new SyncShopsMessage
                {
                    Shops = _mapper.Map<IEnumerable<Shop>>(shops),
                };
                _producer.Publish(message);
            }
        }
    }
}
