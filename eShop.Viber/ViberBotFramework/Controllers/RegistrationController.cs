using eShop.Bots.Common;
using eShop.Viber.Entities;
using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views.Registration;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;
using MassTransit;
using System.Text.RegularExpressions;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class RegistrationController
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+?38)?(\s|-)?0(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])$");

        private readonly IViberService _viberService;
        private readonly IBotContextConverter _botContextConverter;
        private readonly IBus _producer;

        public RegistrationController(
            IViberService viberService,
            IBotContextConverter botContextConverter,
            IBus producer)
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
                var activeContext = _botContextConverter.Serialize(ViberAction.Register, default(string));
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.UserId);
            }

            return null;
        }

        [ConversationStarted(Action = ViberAction.SubscribeToAnnouncer)]
        public async Task<IViberView?> RegisterClient(ConversationStartedContext context, Guid announcerId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberAction.Register, announcerId.ToString());
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.UserId);
            }
            else
            {
                var request = new Messaging.Contracts.Distribution.SubscribeToAnnouncerRequest
                {
                    AccountId = user.AccountId.Value,
                    AnnouncerId = announcerId,
                };

                await _producer.Publish(request);
            }

            return null;
        }

        [ContactMessage(ActiveAction = ViberAction.Register)]
        public async Task<IViberView?> CompleteRegistration(ContactMessageContext context, Guid? announcerId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                await _viberService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Contracts.Viber.RegisterViberUserRequest
                {
                    ViberUserId = user.Id,
                    AnnouncerId = announcerId,
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                };

                await _producer.Publish(request);
            }

            return null;
        }

        [ConversationStarted(Action = ViberAction.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ConversationStartedContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(ViberAction.ConfirmPhoneNumber);
                await _viberService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.UserId);
            }
            else
            {
                // TODO: return already confirmed account view
            }

            return null;
        }

        [ContactMessage(ActiveAction = ViberAction.ConfirmPhoneNumber)]
        public async Task<IViberView?> ConfirmPhoneNumber(ContactMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                await _viberService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Contracts.Viber.RegisterViberUserRequest
                {
                    ViberUserId = user.Id,
                    Name = user.Name,
                    PhoneNumber = phoneNumber,
                    IsConfirmationRequested = true,
                };

                await _producer.Publish(request);
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
    }
}
