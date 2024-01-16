using AutoMapper;
using eShopping.Messaging.Contracts.Distribution;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SetPreferredCurrencyRequestHandler : IConsumer<SetPreferredCurrencyRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ICurrencyService _currencyService;

        public SetPreferredCurrencyRequestHandler(IAccountService accountService, IMapper mapper, ICurrencyService currencyService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _currencyService = currencyService;
        }

        public async Task Consume(ConsumeContext<SetPreferredCurrencyRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var currency = await _currencyService.GetCurrencyAsync(request.CurrencyId);
                if (currency != null)
                {
                    await _accountService.SetPreferredCurrencyAsync(account, currency);

                    var distributionSettings = account.DistributionSettings;
                    var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                    var response = new SetPreferredCurrencyResponse(accountId, true, preferredCurrency);

                    await context.RespondAsync(response);
                }
            }
            else
            {
                var response = new SetPreferredCurrencyResponse(accountId, false, null);

                await context.RespondAsync(response);
            }
        }
    }

}
