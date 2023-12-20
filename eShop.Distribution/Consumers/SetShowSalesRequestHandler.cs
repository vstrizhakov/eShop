using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SetShowSalesRequestHandler : IConsumer<SetShowSalesRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public SetShowSalesRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SetShowSalesRequest> context)
        {
            var request = context.Message;
            var account = await _accountService.GetAccountByIdAsync(request.AccountId);
            if (account != null)
            {
                await _accountService.SetShowSalesAsync(account, request.ShowSales);

                var distributionSettings = account.DistributionSettings;
                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new SetShowSalesResponse(request.AccountId, mappedDistributionSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
