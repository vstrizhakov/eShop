using AutoMapper;
using eShop.Catalog.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Catalog;

namespace eShop.Catalog.MessageHandlers
{
    public class GetCurrenciesRequestHandler : IRequestHandler<GetCurrenciesRequest, GetCurrenciesResponse>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public GetCurrenciesRequestHandler(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        public async Task<GetCurrenciesResponse> HandleRequestAsync(GetCurrenciesRequest request)
        {
            var currencies = await _currencyRepository.GetCurrenciesAsync();
            var mappedCurrencies = _mapper.Map<IEnumerable<Currency>>(currencies);

            var response = new GetCurrenciesResponse(request.AccountId, mappedCurrencies);
            return response;
        }
    }

}
