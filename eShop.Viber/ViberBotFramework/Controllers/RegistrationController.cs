using eShop.Bots.Common;
using eShop.Messaging.Models.Identity;
using eShop.Viber.Entities;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views.Registration;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;
using System.Text.RegularExpressions;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class RegistrationController
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+?38)?(\s|-)?0(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])$");

        private readonly IViberService _viberService;
        private readonly Messaging.IRequestClient _requestClient;
        private readonly IBotContextConverter _botContextConverter;
        private readonly Messaging.IProducer _producer;

        public RegistrationController(
            IViberService viberService,
            Messaging.IRequestClient requestClient,
            IBotContextConverter botContextConverter,
            Messaging.IProducer producer)
        {
            _viberService = viberService;
            _requestClient = requestClient;
            _botContextConverter = botContextConverter;
            _producer = producer;
        }

        [ConversationStarted(Action = ViberContext.RegisterClient)]
        public async Task<IViberView?> RegisterClient(ConversationStartedContext context, Guid providerId)
        {
            var result = await ProcessRegisterClientAsync(context.UserId, providerId);
            return result;
        }

        [TextMessage(Action = ViberContext.RegisterClient)]
        public async Task<IViberView?> RegisterClient(TextMessageContext context, Guid providerId)
        {
            var result = await ProcessRegisterClientAsync(context.UserId, providerId);
            return result;
        }

        [ContactMessage(ActiveAction = ViberContext.RegisterClient)]
        public async Task<IViberView?> CompleteRegistration(ContactMessageContext context, Guid providerId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                var contact = context.Contact;
                var phoneNumber = contact.PhoneNumber;
                phoneNumber = await SetPhoneNumberAsync(user, phoneNumber);

                var request = new Messaging.Models.Viber.RegisterViberUserRequest
                {
                    ViberUserId = user.Id,
                    ProviderId = providerId,
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                };

                var response = await _requestClient.SendAsync(request);
                if (response.IsSuccess)
                {
                    await _viberService.SetAccountIdAsync(user, response.AccountId.Value);

                    return new SuccessfullyRegisteredView(user.ExternalId, response.ProviderEmail);
                }
                else
                {
                    // TODO: handle
                }
            }

            return null;
        }

        [ConversationStarted(Action = ViberContext.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ConversationStartedContext context, string token)
        {
            var result = await ProcessConfirmPhoneNumberAsync(context.UserId, token);
            return result;
        }

        [TextMessage(Action = ViberContext.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(TextMessageContext context, string token)
        {
            var result = await ProcessConfirmPhoneNumberAsync(context.UserId, token);
            return result;
        }

        [ContactMessage(ActiveAction = ViberContext.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ContactMessageContext context, string token)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);

            var contact = context.Contact;
            var phoneNumber = contact.PhoneNumber;

            phoneNumber = await SetPhoneNumberAsync(user, phoneNumber);

            var request = new ConfirmPhoneNumberByViberRequest
            {
                ViberUserId = user.Id,
                PhoneNumber = phoneNumber,
                Token = token,
            };
            _producer.Publish(request);

            return null;
        }

        private async Task<string> SetPhoneNumberAsync(ViberUser user, string phoneNumber)
        {
            phoneNumber = PhoneNumberRegex.Replace(phoneNumber, "+380$4$6$8$10$12$14$16$18$20");

            user.PhoneNumber = phoneNumber;
            user.ActiveContext = null; // await _viberService.SetActiveContextAsync(user, null);

            await _viberService.UpdateUserAsync(user);

            return phoneNumber;
        }

        private async Task<IViberView?> ProcessConfirmPhoneNumberAsync(string contextUserId, string token)
        {
            var user = await _viberService.GetUserByIdAsync(contextUserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberContext.ConfirmPhoneNumber, token);
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(contextUserId);
            }
            else
            {
                var request = new ConfirmPhoneNumberByViberRequest
                {
                    ViberUserId = user.Id,
                    PhoneNumber = user.PhoneNumber!,
                    Token = token,
                };
                _producer.Publish(request);
            }

            return null;
        }

        private async Task<IViberView?> ProcessRegisterClientAsync(string contextUserId, Guid providerId)
        {
            var user = await _viberService.GetUserByIdAsync(contextUserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberContext.RegisterClient, providerId.ToString());
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(contextUserId);
            }
            else
            {
                return new AlreadyRegisteredView();
            }
        }
    }
}
