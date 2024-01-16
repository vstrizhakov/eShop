using AutoMapper;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts.Catalog;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SyncCurrenciesMessageHandler : IConsumer<SyncCurrenciesMessage>
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public SyncCurrenciesMessageHandler(IMapper mapper, ICurrencyService currencyService)
        {
            _mapper = mapper;
            _currencyService = currencyService;
        }

        public async Task Consume(ConsumeContext<SyncCurrenciesMessage> context)
        {
            var command = context.Message;

            var currencies = _mapper.Map<IEnumerable<Entities.Currency>>(command.Currencies);
            await _currencyService.SyncCurrenciesAsync(currencies);
        }
    }
}
