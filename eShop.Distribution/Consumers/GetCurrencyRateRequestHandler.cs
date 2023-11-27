using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetCurrencyRateRequestHandler : IConsumer<GetCurrencyRateRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetCurrencyRateRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCurrencyRateRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            var preferredCurrency = distributionSettings.PreferredCurrency;

            if (preferredCurrency != null)
            {
                var currencyRates = await _distributionSettingsService.GetCurrencyRatesAsync(distributionSettings);
                var currencyRate = currencyRates.FirstOrDefault(e => e.SourceCurrencyId == request.CurrencyId);
                GetCurrencyRateResponse response;

                if (currencyRate == null)
                {
                    response = new GetCurrencyRateResponse
                    {
                        AccountId = accountId,
                    };
                }
                else
                {
                    var mappedPreferredCurrency = _mapper.Map<Currency>(preferredCurrency);
                    var mappedCurrencyRate = _mapper.Map<CurrencyRate>(currencyRate);
                    response = new GetCurrencyRateResponse
                    {
                        AccountId = accountId,
                        Succeeded = true,
                        PreferredCurrency = mappedPreferredCurrency,
                        CurrencyRate = mappedCurrencyRate,
                    };
                }

                await context.RespondAsync(response);
            }
        }
    }

}
