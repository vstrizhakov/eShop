using eShop.Distribution.Services;
using eShop.Messaging.Models.Distribution;
using eShop.Messaging;

namespace eShop.Distribution.MessageHandlers
{
    public class SetComissionAmountRequestHandler : IRequestHandler<SetComissionAmountRequest, SetComissionAmountResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public SetComissionAmountRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task<SetComissionAmountResponse> HandleRequestAsync(SetComissionAmountRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetComissionAmountAsync(distributionSettings, request.Amount);
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionAmountResponse(accountId, comissionSettings.Amount);
                return response;
            }

            return null;
        }
    }

}
