using eShop.Accounts.Repositories;
using eShop.Messaging;
using eShop.Messaging.Models.Identity;

namespace eShop.Accounts.MessageHandlers
{
    public class RegisterIdentityUserRequestHandler : IRequestHandler<RegisterIdentityUserRequest, RegisterIdentityUserResponse>
    {
        private readonly IAccountRepository _repository;

        public RegisterIdentityUserRequestHandler(IAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterIdentityUserResponse> HandleRequestAsync(RegisterIdentityUserRequest request)
        {
            var account = new Entities.Account
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IdentityUserId = request.IdentityUserId,
            };

            await _repository.CreateAccountAsync(account);

            var response = new RegisterIdentityUserResponse
            {
                AccountId = account.Id,
                IdentityUserId = request.IdentityUserId,
            };

            return response;
        }
    }
}
