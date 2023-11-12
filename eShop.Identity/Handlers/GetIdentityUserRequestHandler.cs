using AutoMapper;
using eShop.Identity.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;

namespace eShop.Identity.Handlers
{
    public class GetIdentityUserRequestHandler : IRequestHandler<GetIdentityUserRequest, GetIdentityUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetIdentityUserRequestHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetIdentityUserResponse?> HandleRequestAsync(GetIdentityUserRequest request)
        {
            var response = new GetIdentityUserResponse
            {
                PhoneNumber = request.PhoneNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProviderId = request.ProviderId,
                TelegramUserId = request.TelegramUserId,
                ViberUserId = request.ViberUserId,
                IsConfirmationRequested = request.IsConfirmationRequested,
            };

            var user = await _userRepository.GetByPhoneNumberAsync(request.PhoneNumber);
            if (user != null)
            {
                user.PhoneNumberConfirmed = true;

                await _userRepository.UpdateUserAsync(user);

                response.IdentityUserId = user.Id;
                response.FirstName = user.FirstName;
                response.LastName = user.LastName;
            }

            return response;
        }
    }
}
