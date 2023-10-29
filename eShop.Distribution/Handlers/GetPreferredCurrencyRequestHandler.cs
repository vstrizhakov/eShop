using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class GetPreferredCurrencyRequestHandler : IRequestHandler<GetPreferredCurrencyRequest, GetPreferredCurrencyResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;
        private readonly IMapper _mapper;

        public GetPreferredCurrencyRequestHandler(IDistributionSettingsService distributionSettingsService, IMapper mapper)
        {
            _distributionSettingsService = distributionSettingsService;
            _mapper = mapper;
        }

        public async Task<GetPreferredCurrencyResponse> HandleRequestAsync(GetPreferredCurrencyRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            var preferredCurrency = _mapper.Map<Currency>(distributionSettings.PreferredCurrency);

            var response = new GetPreferredCurrencyResponse(accountId, preferredCurrency);
            return response;
        }
    }

}
