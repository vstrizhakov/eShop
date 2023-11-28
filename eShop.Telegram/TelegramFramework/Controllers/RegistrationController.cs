using eShop.Bots.Common;
using eShop.Telegram.Entities;
using eShop.Telegram.Models;
using eShop.Telegram.Services;
using eShop.Telegram.TelegramFramework.Views;
using eShop.TelegramFramework;
using eShop.TelegramFramework.Attributes;
using eShop.TelegramFramework.Contexts;
using MassTransit;
using System.Text.RegularExpressions;
using Telegram.Bot;

namespace eShop.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class RegistrationController
    {
        private static readonly Regex PhoneNumberRegex = new Regex(@"^(\+?38)?(\s|-)?0(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])(\s|-)?([0-9])$");

        private readonly ITelegramService _telegramService;
        private readonly IBotContextConverter _botContextConverter;
        private readonly IBus _producer;

        public RegistrationController(
            ITelegramService telegramService,
            IBotContextConverter botContextConverter,
            IBus producer)
        {
            _telegramService = telegramService;
            _botContextConverter = botContextConverter;
            _producer = producer;
        }

        [TextMessage(Command = "/start")]
        public async Task<ITelegramView?> Start(TextMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(TelegramAction.Register, default(string));
                await _telegramService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.ChatId);
            }
            else
            {
                return new WelcomeView(context.ChatId, null);
            }
        }

        [TextMessage(Action = TelegramAction.SubscribeToAnnouncer, Command = "/start")]
        public async Task<ITelegramView?> SubscribeToAnnouncer(TextMessageContext context, Guid announcerId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(TelegramAction.Register, announcerId.ToString());
                await _telegramService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.ChatId);
            }
            else
            {
                var request = new Messaging.Contracts.Distribution.SubscribeToAnnouncerRequest
                {
                    TelegramUserId = user.Id,
                    AnnouncerId = announcerId,
                };

                await _producer.Publish(request);
            }

            return null;
        }

        [ContactMessage(Action = TelegramAction.Register)]
        public async Task<ITelegramView?> CompleteRegistration(ContactMessageContext context, Guid? announcerId)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId == null)
            {
                await _telegramService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Contracts.Telegram.RegisterTelegramUserRequest
                {
                    TelegramUserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = phoneNumber,
                    AnnouncerId = announcerId,
                };

                await _producer.Publish(request);
            }

            return null;
        }

        [TextMessage(Action = TelegramAction.ConfirmPhoneNumber, Command = "/start")]
        public async Task<ITelegramView?> ConfirmPhoneNumber(TextMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId == null)
            {
                var activeContext = _botContextConverter.Serialize(TelegramAction.ConfirmPhoneNumber);
                await _telegramService.SetActiveContextAsync(user, activeContext);

                return new FinishRegistrationView(context.ChatId);
            }
            else
            {
                // TODO: return already confirmed account view
            }

            return null;
        }

        [ContactMessage(Action = TelegramAction.ConfirmPhoneNumber)]
        public async Task<ITelegramView?> ConfirmPhoneNumber(ContactMessageContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId == null)
            {
                await _telegramService.SetActiveContextAsync(user, null);

                var contact = context.Contact;
                var phoneNumber = await SetPhoneNumberAsync(user, contact.PhoneNumber);

                var request = new Messaging.Contracts.Telegram.RegisterTelegramUserRequest
                {
                    TelegramUserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = phoneNumber,
                    IsConfirmationRequested = true,
                };

                await _producer.Publish(request);
            }

            return null;
        }

        private async Task<string> SetPhoneNumberAsync(TelegramUser user, string phoneNumber)
        {
            phoneNumber = PhoneNumberRegex.Replace(phoneNumber, "+380$4$6$8$10$12$14$16$18$20");

            user.PhoneNumber = phoneNumber;

            await _telegramService.UpdateUserAsync(user);

            return phoneNumber;
        }
    }
}
