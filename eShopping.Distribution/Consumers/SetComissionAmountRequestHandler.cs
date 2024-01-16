using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SetComissionAmountRequestHandler : IConsumer<SetComissionAmountRequest>
    {
        private readonly IAccountService _accountService;

        public SetComissionAmountRequestHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task Consume(ConsumeContext<SetComissionAmountRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                await _accountService.SetComissionAmountAsync(account, request.Amount);
                var comissionSettings = distributionSettings.ComissionSettings;

                var response = new SetComissionAmountResponse(accountId, comissionSettings.Amount);

                await context.RespondAsync(response);
            }
        }
    }

}
