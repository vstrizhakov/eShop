using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class GetComissionAmountRequestHandler : IRequestHandler<GetComissionAmountRequest, GetComissionAmountResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public GetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task<GetComissionAmountResponse> HandleRequestAsync(GetComissionAmountRequest message)
        {
            var accountId = message.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);
            if (distributionSettings != null)
            {
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new GetComissionAmountResponse(accountId, comissionSettings.Amount);
                return response;
            }

            return null;
        }
    }
}
