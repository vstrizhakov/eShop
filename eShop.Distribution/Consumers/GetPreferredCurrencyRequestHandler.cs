using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetPreferredCurrencyRequestHandler : IConsumer<GetPreferredCurrencyRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetPreferredCurrencyRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetPreferredCurrencyRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);

                var response = new GetPreferredCurrencyResponse(accountId, preferredCurrency);
                await context.RespondAsync(response);
            }
        }
    }
}
