using AutoMapper;
using eShop.Accounts.Entities;
using eShop.Accounts.Exceptions;
using eShop.Accounts.Services;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using eShop.Messaging.Models.Telegram;
using eShop.Messaging.Models.Viber;

namespace eShop.Accounts.Handlers
{
    public class GetIdentityUserResponseHandler : IMessageHandler<GetIdentityUserResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IProducer _producer;

        public GetIdentityUserResponseHandler(IAccountService accountService, IMapper mapper, IProducer producer)
        {
            _accountService = accountService;
            _mapper = mapper;
            _producer = producer;
        }

        public async Task HandleMessageAsync(GetIdentityUserResponse response)
        {
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

                    var @event = new Messaging.Models.AccountRegisteredEvent
                    {
                        Account = _mapper.Map<Messaging.Models.Account>(account),
                        AnnouncerId = announcerId,
                    };
                    _producer.Publish(@event);

                    if (viberUserId.HasValue)
                    {
                        var responseMessage = new RegisterViberUserResponse
                        {
                            IsSuccess = true,
                            ViberUserId = viberUserId.Value,
                            AccountId = account.Id,
                            IsConfirmationRequested = response.IsConfirmationRequested,
                        };

                        _producer.Publish(responseMessage);
                    }

                    if (telegramUserId.HasValue)
                    {
                        var responseMessage = new RegisterTelegramUserResponse
                        {
                            TelegramUserId = telegramUserId.Value,
                            AccountId = account.Id,
                            IsConfirmationRequested = response.IsConfirmationRequested,
                        };

                        _producer.Publish(responseMessage);
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
