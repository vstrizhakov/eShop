using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetDistributionSettingsRequestHandler : IConsumer<GetDistributionSettingsRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetDistributionSettingsRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetDistributionSettingsRequest> context)
        {
            var request = context.Message;
            var account = await _accountService.GetAccountByIdAsync(request.AccountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var mappedDistributionSettings = _mapper.Map<DistributionSettings>(distributionSettings);
                var response = new GetDistributionSettingsResponse(request.AccountId, mappedDistributionSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
