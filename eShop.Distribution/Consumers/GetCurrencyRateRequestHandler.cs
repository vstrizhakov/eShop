using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetCurrencyRateRequestHandler : IConsumer<GetCurrencyRateRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetCurrencyRateRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCurrencyRateRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var preferredCurrency = distributionSettings.PreferredCurrency;
                if (preferredCurrency != null)
                {
                    var currencyRates = await _accountService.GetCurrencyRatesAsync(account);
                    var currencyRate = currencyRates.FirstOrDefault(e => e.SourceCurrency.Id == request.CurrencyId);
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

}
