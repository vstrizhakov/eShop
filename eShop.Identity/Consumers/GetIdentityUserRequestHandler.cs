using eShop.Identity.Entities;
using eShop.Identity.Repositories;
using eShop.Messaging.Contracts.Identity;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace eShop.Identity.Consumers
{
    public class GetIdentityUserRequestHandler : IConsumer<GetIdentityUserRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public GetIdentityUserRequestHandler(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task Consume(ConsumeContext<GetIdentityUserRequest> context)
        {
            var request = context.Message;

            var phoneNumber = request.PhoneNumber;
            var response = new GetIdentityUserResponse
            {
                PhoneNumber = phoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                AnnouncerId = request.AnnouncerId,
                TelegramUserId = request.TelegramUserId,
                ViberUserId = request.ViberUserId,
                IsConfirmationRequested = request.IsConfirmationRequested,
            };

            var user = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            if (user != null)
            {
                user.PhoneNumberConfirmed = true;

                await _userRepository.UpdateUserAsync(user);

                response.FirstName = user.FirstName;
                response.LastName = user.LastName;
            }
            else
            {
                user = new User
                {
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = true,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                };
                await _userManager.CreateAsync(user);
            }

            response.IdentityUserId = user.Id;

            await context.Publish(response);
        }
    }
}
