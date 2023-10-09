using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Catalog;

namespace eShop.Distribution.MessageHandlers
{
    public class SyncCurrenciesMessageHandler : IMessageHandler<SyncCurrenciesMessage>
    {
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public SyncCurrenciesMessageHandler(IMapper mapper, ICurrencyService currencyService)
        {
            _mapper = mapper;
            _currencyService = currencyService;
        }

        public async Task HandleMessageAsync(SyncCurrenciesMessage message)
        {
            var currencies = _mapper.Map<IEnumerable<Entities.Currency>>(message.Currencies);
            await _currencyService.SyncCurrenciesAsync(currencies);
        }
    }
}
