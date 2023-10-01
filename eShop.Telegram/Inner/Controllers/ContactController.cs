using eShop.Messaging;
using eShop.Telegram.Inner.Contexts;
using eShop.Telegram.Inner.Views;
using eShop.Telegram.Services;

namespace eShop.Telegram.Inner.Controllers
{
    [TelegramController(Context = TelegramContext.ContactMessage)]
    public class ContactController : TelegramControllerBase
    {
        private readonly ITelegramService _telegramService;
        private readonly IProducer _producer;

        public ContactController(ITelegramService telegramService, IProducer producer)
        {
            _telegramService = telegramService;
            _producer = producer;
        }

        public async Task<ITelegramView?> ProcessAsync(ContactMessageContext context)
        {
            var contact = context.Contact;

            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            var providerId = user!.RegistrationProviderId;
            if (providerId != null)
            {
                var phoneNumber = contact.PhoneNumber;

                user.PhoneNumber = phoneNumber;
                user.RegistrationProviderId = null;

                await _telegramService.UpdateUser(user);

                var internalMessage = new Messaging.Models.TelegramUserCreateAccountRequestMessage
                {
                    TelegramUserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = phoneNumber,
                    ProviderId = providerId.Value,
                };
                _producer.Publish(internalMessage);

                return new WelcomeView(context.ChatId);
            }

            return null;
        }
    }
}
