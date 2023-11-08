using eShop.Identity.Entities;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace eShop.Identity.Handlers
{
    public class RegisterIdentityUserResponseHandler : IMessageHandler<RegisterIdentityUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IProducer _producer;

        public RegisterIdentityUserResponseHandler(UserManager<User> userManager, IProducer producer)
        {
            _userManager = userManager;
            _producer = producer;
        }

        public async Task HandleMessageAsync(RegisterIdentityUserResponse response)
        {
            var user = await _userManager.FindByIdAsync(response.IdentityUserId);
            if (user != null)
            {
                var accountId = response.AccountId;
                user.AccountId = accountId;

                // TODO: Handle failure
                await _userManager.UpdateAsync(user);

                var viberUserId = response.ViberUserId;
                if (viberUserId.HasValue)
                {
                    var confirmationResponse = new ConfirmPhoneNumberByViberResponse
                    {
                        RequestId = response.RequestId,
                        ViberUserId = viberUserId.Value,
                        Succeeded = true,
                        AccountId = accountId,
                    };
                    _producer.Publish(confirmationResponse);
                }
            }
        }
    }
}
