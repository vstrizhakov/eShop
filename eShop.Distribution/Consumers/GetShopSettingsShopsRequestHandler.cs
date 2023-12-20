using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution.ShopSettings;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class GetShopSettingsShopsRequestHandler : IConsumer<GetShopSettingsShopsRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetShopSettingsShopsRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetShopSettingsShopsRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountByIdAsync(accountId);
            if (account != null)
            {
                var shops = await _accountService.GetShopsAsync(account);
                var mappedShops = _mapper.Map<IEnumerable<Shop>>(shops);

                var response = new GetShopSettingsShopsResponse(accountId, mappedShops);

                await context.RespondAsync(response);
            }
        }
    }

}
