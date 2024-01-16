using AutoMapper;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SetCurrencyRateRequestHandler : IConsumer<SetCurrencyRateRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public SetCurrencyRateRequestHandler(IAccountService accountService, IMapper mapper, ICurrencyService currencyService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _currencyService = currencyService;
        }

        public async Task Consume(ConsumeContext<SetCurrencyRateRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var currency = await _currencyService.GetCurrencyAsync(request.CurrencyId);
                if (currency != null)
                {
                    await _accountService.SetCurrencyRateAsync(account, currency, request.Rate);
                    var currencyRates = await _accountService.GetCurrencyRatesAsync(account);

                    var distributionSettings = account.DistributionSettings;
                    var mappedPreferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                    var mappedCurrencyRates = _mapper.Map<IEnumerable<CurrencyRate>>(currencyRates);
                    var response = new SetCurrencyRateResponse(accountId, mappedPreferredCurrency, mappedCurrencyRates);

                    await context.RespondAsync(response);
                }
            }
        }
    }

}
