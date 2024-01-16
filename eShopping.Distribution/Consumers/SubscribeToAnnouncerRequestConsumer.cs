using AutoMapper;
using eShopping.Distribution.Entities;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts.Distribution;
using MassTransit;

namespace eShopping.Distribution.Consumers
{
    public class SubscribeToAnnouncerRequestConsumer : IConsumer<SubscribeToAnnouncerRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public SubscribeToAnnouncerRequestConsumer(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SubscribeToAnnouncerRequest> context)
        {
            var request = context.Message;
            var accountId = request.AccountId;
            var telegramUserId = request.TelegramUserId;
            var viberUserId = request.ViberUserId;

            Account? account = null;
            if (accountId.HasValue)
            {
                account = await _accountService.GetAccountByIdAsync(accountId.Value);
            }
            else if (telegramUserId.HasValue)
            {
                account = await _accountService.GetAccountByTelegramUserIdAsync(telegramUserId.Value);
            }
            else if (viberUserId.HasValue)
            {
                account = await _accountService.GetAccountByViberUserIdAsync(viberUserId.Value);
            }

            if (account != null)
            {
                var response = new SubscribeToAnnouncerResponse
                {
                    AccountId = accountId,
                    TelegramUserId = telegramUserId,
                    ViberUserId = viberUserId,
                };

                var announcer = await _accountService.GetAccountByIdAsync(request.AnnouncerId);
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
