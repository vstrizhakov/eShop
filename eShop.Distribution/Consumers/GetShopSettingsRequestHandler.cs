using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetShopSettingsRequestHandler : IConsumer<GetShopSettingsRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetShopSettingsRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetShopSettingsRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var distributionSettings = account.DistributionSettings;
                var shopSettings = _mapper.Map<ShopSettings>(distributionSettings.ShopSettings);
                var response = new GetShopSettingsResponse(accountId, shopSettings);

                await context.RespondAsync(response);
            }
        }
    }
}
