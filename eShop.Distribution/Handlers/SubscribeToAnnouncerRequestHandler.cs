using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Distribution;

namespace eShop.Distribution.Handlers
{
    public class SubscribeToAnnouncerRequestHandler : IRequestHandler<SubscribeToAnnouncerRequest, SubscribeToAnnouncerResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public SubscribeToAnnouncerRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<SubscribeToAnnouncerResponse?> HandleRequestAsync(SubscribeToAnnouncerRequest request)
        {
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountAsync(accountId);
            if (account != null)
            {
                var response = new SubscribeToAnnouncerResponse
                {
                    AccountId = accountId,
                };

                var announcer = await _accountService.GetAccountAsync(request.AccountId);
                if (announcer != null)
                {
                    await _accountService.SubscribeToAnnouncerAsync(account, announcer);

                    response.Succeeded = true;
                    response.Announcer = _mapper.Map<Announcer>(announcer);
                }

                return response;
            }

            return null;
        }
    }
}
