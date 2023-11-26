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
        private readonly IBotContextConverter _botContextConverter;
        private readonly Messaging.IProducer _producer;

        public RegistrationController(
            IViberService viberService,
            IBotContextConverter botContextConverter,
            Messaging.IProducer producer)
        {
            _viberService = viberService;
            _botContextConverter = botContextConverter;
            _producer = producer;
        }

        [ConversationStarted]
        public async Task<IViberView?> Register(ConversationStartedContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                return new FinishRegistrationView(context.UserId);
            }

            return null;
        }

        [ContactMessage]
        public async Task<IViberView?> CompleteRegistration(ContactMessageContext context)
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
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                };

                _producer.Publish(request);
            }

            return null;
        }

        [ConversationStarted(Action = ViberContext.RegisterClient)]
        public async Task<IViberView?> RegisterClient(ConversationStartedContext context, Guid announcerId)
        {
            var result = await ProcessRegisterClientAsync(context.UserId, announcerId);
            return result;
        }

        [TextMessage(Action = ViberContext.RegisterClient)]
        public async Task<IViberView?> RegisterClient(TextMessageContext context, Guid announcerId)
        {
            var result = await ProcessRegisterClientAsync(context.UserId, announcerId);
            return result;
        }

        [ContactMessage(ActiveAction = ViberContext.RegisterClient)]
        public async Task<IViberView?> CompleteClientRegistration(ContactMessageContext context, Guid announcerId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                await _viberService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Models.Viber.RegisterViberUserRequest
                {
                    ViberUserId = user.Id,
                    AnnouncerId = announcerId,
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                };

                _producer.Publish(request);
            }

            return null;
        }

        [ConversationStarted(Action = ViberContext.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ConversationStartedContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberContext.ConfirmPhoneNumber);
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.UserId);
            }
            else
            {
                // TODO: return already confirmed account view
            }

            return null;
        }

        [ContactMessage(ActiveAction = ViberContext.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ContactMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                await _viberService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Models.Viber.RegisterViberUserRequest
                {
                    ViberUserId = user.Id,
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                    IsConfirmationRequested = true,
                };

                _producer.Publish(request);
            }

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

        private async Task<IViberView?> ProcessRegisterClientAsync(string contextUserId, Guid announcerId)
        {
            var user = await _viberService.GetUserByIdAsync(contextUserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberContext.RegisterClient, announcerId.ToString());
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
