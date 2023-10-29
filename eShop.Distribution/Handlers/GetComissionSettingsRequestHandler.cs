using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class GetComissionSettingsRequestHandler : IRequestHandler<GetComissionSettingsRequest, GetComissionSettingsResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public GetComissionSettingsRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task<GetComissionSettingsResponse> HandleRequestAsync(GetComissionSettingsRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            GetComissionSettingsResponse response = null;

            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;
                response = new GetComissionSettingsResponse(accountId, comissionSettings.Amount);
            }

            return response;
        }
    }

}
