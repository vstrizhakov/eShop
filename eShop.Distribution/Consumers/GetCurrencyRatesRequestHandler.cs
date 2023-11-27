using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetCurrencyRatesRequestHandler : IConsumer<GetCurrencyRatesRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetCurrencyRatesRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCurrencyRatesRequest> context)
        {
            var request = context.Message;

            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings?.PreferredCurrencyId != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);
                var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);

                var response = new GetCurrencyRatesResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);

                await context.RespondAsync(response);
            }
        }
    }

}
