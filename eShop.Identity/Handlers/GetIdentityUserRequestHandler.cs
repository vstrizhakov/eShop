using AutoMapper;
using eShop.Identity.Entities;
using eShop.Identity.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace eShop.Identity.Handlers
{
    public class GetIdentityUserRequestHandler : IRequestHandler<GetIdentityUserRequest, GetIdentityUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public GetIdentityUserRequestHandler(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetIdentityUserResponse?> HandleRequestAsync(GetIdentityUserRequest request)
        {
            var phoneNumber = request.PhoneNumber;
            var response = new GetIdentityUserResponse
            {
                PhoneNumber = phoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProviderId = request.ProviderId,
                TelegramUserId = request.TelegramUserId,
                ViberUserId = request.ViberUserId,
                IsConfirmationRequested = request.IsConfirmationRequested,
            };

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
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
                    UserName = phoneNumber,
                    PhoneNumber = phoneNumber,
                    PhoneNumberConfirmed = true,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                };
                await _userManager.CreateAsync(user);
            }

            response.IdentityUserId = user.Id;

            return response;
        }
    }
}
