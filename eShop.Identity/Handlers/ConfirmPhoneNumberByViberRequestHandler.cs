using eShop.Identity.Entities;
using eShop.Identity.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace eShop.Identity.Handlers
{
    public class ConfirmPhoneNumberByViberRequestHandler : IRequestHandler<ConfirmPhoneNumberByViberRequest, ConfirmPhoneNumberByViberResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IProducer _producer;

        public ConfirmPhoneNumberByViberRequestHandler(IUserRepository userRepository, UserManager<User> userManager, IProducer producer)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _producer = producer;
        }

        public async Task<ConfirmPhoneNumberByViberResponse> HandleRequestAsync(ConfirmPhoneNumberByViberRequest request)
        {
            var response = new ConfirmPhoneNumberByViberResponse
            {
                ViberUserId = request.ViberUserId,
            };

            var phoneNumber = request.PhoneNumber;
            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            if (user != null)
            {
                var result = await _userManager.ChangePhoneNumberAsync(user, phoneNumber, request.Token);
                if (result.Succeeded)
                {
                    var message = new RegisterIdentityUserRequest
                    {
                        RequestId = request.RequestId,
                        IdentityUserId = user.Id,
                        ViberUserId = request.ViberUserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber!,
                    };
                    _producer.Publish(message);

                    response = null;
                }
                else
                {
                    response.IsPhoneNumberInvalid = false;
                    response.IsTokenInvalid = true;
                }
            }
            else
            {
                response.IsPhoneNumberInvalid = true;
                response.IsTokenInvalid = false;
            }

            return response;
        }
    }
}
