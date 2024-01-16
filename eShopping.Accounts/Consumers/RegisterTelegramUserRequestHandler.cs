using eShopping.Messaging.Contracts.Telegram;
using eShopping.Messaging.Contracts.Identity;
using MassTransit;
using eShopping.Messaging.Contracts;
using AutoMapper;
using eShopping.Accounts.Entities;
using eShopping.Accounts.Services;
using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Accounts.Consumers
{
    public class RegisterTelegramUserRequestHandler : IConsumer<RegisterTelegramUserRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RegisterTelegramUserRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<RegisterTelegramUserRequest> context)
        {
            var request = context.Message;

            var phoneNumber = request.PhoneNumber;
            var telegramUserId = request.TelegramUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkTelegramUserAsync(account, telegramUserId);

                var @event = new AccountUpdatedEvent
                {
                    Account = _mapper.Map<Messaging.Contracts.Account>(account),
                };
                await context.Publish(@event);

                Entities.Account? announcer = null;

                var announcerId = request.AnnouncerId;
                if (announcerId.HasValue)
                {
                    announcer = await _accountService.GetAccountByIdAsync(announcerId.Value);
                    if (announcer != null)
                    {
                        var command = new SubscribeToAnnouncerRequest
                        {
                            AccountId = account.Id,
                            AnnouncerId = announcerId.Value,
                        };
                        await context.Publish(command);
                    }
                }

                var response = new RegisterTelegramUserResponse
                {
                    TelegramUserId = telegramUserId,
                    AccountId = account.Id,
                    Announcer = _mapper.Map<Announcer>(announcer),
                };
                await context.Publish(response);
            }
            else
            {
                var getIdentityUserRequest = new GetIdentityUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    AnnouncerId = request.AnnouncerId,
                    TelegramUserId = telegramUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
                await context.Publish(getIdentityUserRequest);
            }
        }
    }
}
