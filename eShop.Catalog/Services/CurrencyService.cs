using AutoMapper;
using eShop.Catalog.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;

namespace eShop.Catalog.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public CurrencyService(ICurrencyRepository currencyRepository, IMapper mapper, IProducer producer)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task SyncCurrenciesAsync()
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
        }
    }
}
