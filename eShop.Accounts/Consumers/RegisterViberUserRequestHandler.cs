using eShop.Accounts.Services;
using eShop.Messaging.Contracts.Viber;
using eShop.Messaging.Contracts.Identity;
using MassTransit;
using eShop.Messaging.Contracts.Distribution;
using eShop.Messaging.Contracts;
using AutoMapper;

namespace eShop.Accounts.Consumers
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

                var announcerId = request.AnnouncerId;
                if (announcerId.HasValue)
                {
                    var command = new SubscribeToAnnouncerRequest
                    {
                        AccountId = account.Id,
                        AnnouncerId = announcerId.Value,
                    };
                    await context.Publish(command);
                }

                var response = new RegisterViberUserResponse
                {
                    IsSuccess = true,
                    AccountId = account.Id,
                    ViberUserId = viberUserId,
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
