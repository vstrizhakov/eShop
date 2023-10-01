using AutoMapper;
using eShop.Catalog.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;

namespace eShop.Catalog.MessageHandlers
{
    public class GetCurrenciesRequestHandler : IMessageHandler<GetCurrenciesRequest>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public GetCurrenciesRequestHandler(ICurrencyRepository currencyRepository, IMapper mapper, IProducer producer)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetCurrenciesRequest message)
        {
            var currencies = await _currencyRepository.GetCurrenciesAsync();

            var mappedCurrencies = _mapper.Map<IEnumerable<Currency>>(currencies);
            var response = new GetCurrenciesResponse(message.AccountId, mappedCurrencies);
            _producer.Publish(response);
        }
    }
}
