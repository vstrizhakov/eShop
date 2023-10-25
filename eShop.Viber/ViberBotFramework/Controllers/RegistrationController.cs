using eShop.Viber.Models;
using eShop.Viber.Services;
using eShop.Viber.ViberBotFramework.Views;
using eShop.ViberBot.Framework;
using eShop.ViberBot.Framework.Attributes;
using eShop.ViberBot.Framework.Contexts;

namespace eShop.Viber.ViberBotFramework.Controllers
{
    [ViberController]
    public class RegistrationController
    {
        private readonly IViberService _viberService;
        private readonly Messaging.IRequestClient _requestClient;

        public RegistrationController(IViberService viberService, Messaging.IRequestClient requestClient)
        {
            _viberService = viberService;
            _requestClient = requestClient;
        }

        [ConversationStarted(Action = ViberContext.RegisterClient)]
        public async Task<IViberView?> ConversationStarted(ConversationStartedContext context, Guid providerId)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                await _viberService.SetRegistrationProviderIdAsync(user, providerId);

                return new FinishRegistrationView();
            }
            else
            {
                return new AlreadyRegisteredView();
            }
        }

        [ContactMessage]
        public async Task<IViberView?> CompleteRegistration(ContactMessageContext context)
        {
            var user = await _viberService.GetUserByIdAsync(context.UserId);
            if (user!.AccountId == null)
            {
                var providerId = user.RegistrationProviderId;
                if (providerId.HasValue)
                {
                    var contact = context.Contact;
                    var phoneNumber = contact.PhoneNumber;

                    user.PhoneNumber = phoneNumber;
                    user.RegistrationProviderId = null;

                    await _viberService.UpdateUserAsync(user);

                    var request = new Messaging.Models.Viber.RegisterViberUserRequest
                    {
                        ViberUserId = user.Id,
                        ProviderId = providerId.Value,
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
            }

            return null;
        }
    }
}
