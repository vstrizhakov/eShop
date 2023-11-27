using AutoMapper;
using eShop.Distribution.Services;
using eShop.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShop.Distribution.Consumers
{
    public class SubscribeToAnnouncerRequestHandler : IConsumer<SubscribeToAnnouncerRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public SubscribeToAnnouncerRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SubscribeToAnnouncerRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var account = await _accountService.GetAccountAsync(accountId);
            if (account != null)
            {
                var response = new SubscribeToAnnouncerResponse
                {
                    CorrelationId = request.CorrelationId,
                    AccountId = accountId,
                };

                var announcer = await _accountService.GetAccountAsync(request.AccountId);
                if (announcer != null)
                {
                    await _accountService.SubscribeToAnnouncerAsync(account, announcer);

                    response.Succeeded = true;
                    response.Announcer = _mapper.Map<Announcer>(announcer);
                }

                await context.Publish(response);
            }
        }
    }
}
