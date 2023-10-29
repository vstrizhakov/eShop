using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class SetPreferredCurrencyRequestHandler : IRequestHandler<SetPreferredCurrencyRequest, SetPreferredCurrencyResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public SetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<SetPreferredCurrencyResponse> HandleRequestAsync(SetPreferredCurrencyRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetPreferredCurrencyAsync(distributionSettings, request.CurrencyId);
                var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);
                var response = new SetPreferredCurrencyResponse(accountId, true, preferredCurrency);
                return response;
            }
            else
            {
                var response = new SetPreferredCurrencyResponse(accountId, false, null);
                return response;
            }
        }
    }

}
