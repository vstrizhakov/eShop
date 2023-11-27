using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetPreferredCurrencyRequestHandler : IConsumer<SetPreferredCurrencyRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetPreferredCurrencyRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetPreferredCurrencyAsync(distributionSettings, request.CurrencyId);
                var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var response = new SetPreferredCurrencyResponse(accountId, true, preferredCurrency);

                await context.RespondAsync(response);
            }
            else
            {
                var response = new SetPreferredCurrencyResponse(accountId, false, null);

                await context.RespondAsync(response);
            }
        }
    }

}
