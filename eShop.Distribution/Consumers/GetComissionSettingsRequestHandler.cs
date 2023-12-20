using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetComissionSettingsRequestHandler : IConsumer<GetComissionSettingsRequest>
    {
        private readonly IAccountService _accountService;

        public GetComissionSettingsRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<GetComissionSettingsRequest> context)
        {
            var request = context.Message;

            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var comissionSettings = distributionSettings.ComissionSettings;
                var response = new GetComissionSettingsResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }

}
