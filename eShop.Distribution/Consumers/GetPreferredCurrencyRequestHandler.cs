using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts;
using eShop.Messaging.Contracts.Distribution;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetPreferredCurrencyRequestHandler : IConsumer<GetPreferredCurrencyRequest>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetPreferredCurrencyRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);

                var response = new GetPreferredCurrencyResponse(accountId, preferredCurrency);
                await context.RespondAsync(response);
            }
        }
    }
}
