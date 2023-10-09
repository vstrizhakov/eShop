using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.MessageHandlers
{
    public class SetComissionShowRequestHandler : IRequestHandler<SetComissionShowRequest, SetComissionShowResponse>
    {
        private readonly IDistributionSettingsService _distributionSettingsService;

        public SetComissionShowRequestHandler(IDistributionSettingsService distributionSettingsService)
        {
            _distributionSettingsService = distributionSettingsService;
        }

        public async Task<SetComissionShowResponse> HandleRequestAsync(SetComissionShowRequest request)
        {
            var accountId = request.AccountId;
            var distributionSettings = await _distributionSettingsService.GetDistributionSettingsAsync(accountId);

            if (distributionSettings != null)
            {
                distributionSettings = await _distributionSettingsService.SetComissionShowAsync(distributionSettings, request.Show);
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionShowResponse(accountId, comissionSettings.Show, comissionSettings.Amount);
                return response;
            }

            return null;
        }
    }

}
