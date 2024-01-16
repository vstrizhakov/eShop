using AutoMapper;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class GetCurrencyRatesRequestHandler : IConsumer<GetCurrencyRatesRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetCurrencyRatesRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetCurrencyRatesRequest> context)
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
                    var mappedPreferredCurrency = _mapper.Map<Currency>(preferredCurrency);
                    var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);

                    var response = new GetCurrencyRatesResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);

                    await context.RespondAsync(response);
                }
            }
        }
    }

}
