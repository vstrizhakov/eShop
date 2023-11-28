using AutoMapper;
using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging.Contracts.Identity;
using eShop.Messaging.Contracts.Telegram;
using eShop.Messaging.Contracts.Viber;
using MassTransit;

namespace eShop.Accounts.Consumers
{
    public class GetIdentityUserResponseConsumer : IConsumer<GetIdentityUserResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetIdentityUserResponseConsumer(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetIdentityUserResponse> context)
        {
            var response = context.Message;

            var phoneNumber = response.PhoneNumber;

            var account = await _accountService.GetAccountByPhoneNumberAsync(phoneNumber);
            if (account == null)
            {
                var telegramUserId = response.TelegramUserId;
                var viberUserId = response.ViberUserId;

                account = new Account
                {
                    PhoneNumber = phoneNumber,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    IdentityUserId = response.IdentityUserId,
                    TelegramUserId = telegramUserId,
                    ViberUserId = viberUserId,
                };

                try
                {
                    account = await _accountService.RegisterAccountAsync(phoneNumber, account);

                    var announcerId = response.AnnouncerId;
                    if (announcerId.HasValue)
                    {
                        var announcer = await _accountService.GetAccountByIdAsync(announcerId.Value);
                        if (announcer == null)
                        {
                            announcerId = null;
                            // TODO: provide feedback
                            //throw new ProviderNotExistsException();
                        }
                    }

                    var @event = new Messaging.Contracts.AccountRegisteredEvent
                    {
                        Account = _mapper.Map<Messaging.Contracts.Account>(account),
                        AnnouncerId = announcerId,
                    };
                    await context.Publish(@event);

                    if (viberUserId.HasValue)
                    {
                        var responseMessage = new RegisterViberUserResponse
                        {
                            IsSuccess = true,
                            ViberUserId = viberUserId.Value,
                            AccountId = account.Id,
                            IsConfirmationRequested = response.IsConfirmationRequested,
                        };

                        await context.Publish(responseMessage);
                    }

                    if (telegramUserId.HasValue)
                    {
                        var responseMessage = new RegisterTelegramUserResponse
                        {
                            TelegramUserId = telegramUserId.Value,
                            AccountId = account.Id,
                            IsConfirmationRequested = response.IsConfirmationRequested,
                        };

                        await context.Publish(responseMessage);
                    }
                }
                catch (AccountAlreadyRegisteredException)
                {
                    // TODO: Publish message with error
                }
                catch (ProviderNotExistsException)
                {
                    // TODO: Publish message with error
                }
            }
            else
            {
                // TODO: handle
            }
        }
    }
}
