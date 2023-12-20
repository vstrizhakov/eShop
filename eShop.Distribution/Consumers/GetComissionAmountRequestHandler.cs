using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetComissionAmountRequestHandler : IConsumer<GetComissionAmountRequest>
    {
        private readonly IAccountService _accountService;

        public GetComissionAmountRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<GetComissionAmountRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new GetComissionAmountResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }
}
