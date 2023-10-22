using eShop.Identity.Entities;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace eShop.Identity.MessageHandlers
{
    public class IdentityUserCreateAccountResponseMessageHandler : IMessageHandler<RegisterIdentityUserResponse>
    {
        private readonly UserManager<User> _userManager;

        public IdentityUserCreateAccountResponseMessageHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task HandleMessageAsync(RegisterIdentityUserResponse message)
        {
            var user = await _userManager.FindByIdAsync(message.IdentityUserId);
            if (user != null)
            {
                user.AccountId = message.AccountId;

                // TODO: Handle failure
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
