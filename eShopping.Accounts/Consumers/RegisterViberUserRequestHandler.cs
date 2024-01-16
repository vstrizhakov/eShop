using eShopping.Messaging.Contracts.Identity;
using MassTransit;
using eShopping.Messaging.Contracts;
using AutoMapper;
using eShopping.Messaging.Contracts.Viber;
using eShopping.Accounts.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Distribution;

namespace eShopping.Accounts.Consumers
{
    public class RegisterViberUserRequestHandler : IConsumer<RegisterViberUserRequest>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RegisterViberUserRequestHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<RegisterViberUserRequest> context)
        {
            var request = context.Message;

            var phoneNumber = request.PhoneNumber;
            var viberUserId = request.ViberUserId;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account != null)
            {
                await _accountService.LinkViberUserAsync(account, viberUserId);

                var @event = new AccountUpdatedEvent
                {
                    Account = _mapper.Map<Account>(account),
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
                    // TODO: handle announcer not exists
                }

                var response = new RegisterViberUserResponse
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ViberUserId = viberUserId,
                    Announcer = _mapper.Map<Announcer>(announcer),
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
                await context.Publish(response);
            }
            else
            {
                var getIdentityUserRequest = new GetIdentityUserRequest
                {
                    PhoneNumber = phoneNumber,
                    FirstName = request.Name,
                    AnnouncerId = request.AnnouncerId,
                    ViberUserId = viberUserId,
                    IsConfirmationRequested = request.IsConfirmationRequested,
                };
                await context.Publish(getIdentityUserRequest);
            }
        }
    }
}
