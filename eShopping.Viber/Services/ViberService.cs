using eShopping.Messaging;
using eShopping.Viber.Entities;
using eShopping.Viber.Repositories;
using MassTransit;

namespace eShopping.Viber.Services
{
    public class ViberService : IViberService
    {
        private readonly IViberUserRepository _viberUserRepository;
        private readonly IBus _producer;

        public ViberService(IViberUserRepository viberUserRepository, IBus producer)
        {
            _viberUserRepository = viberUserRepository;
            _producer = producer;
        }

        public async Task<ViberUser?> GetUserByAccountIdAsync(Guid accountId)
        {
            var user = await _viberUserRepository.GetViberUserByAccountIdAsync(accountId);
            return user;
        }

        public async Task<ViberUser?> GetUserByIdAsync(string userId)
        {
            var user = await _viberUserRepository.GetViberUserByExternalIdAsync(userId);
            return user;
        }

        public async Task<ViberUser?> GetUserByViberUserIdAsync(Guid viberUserId)
        {
            var user = await _viberUserRepository.GetViberUserByIdAsync(viberUserId);
            return user;
        }

        public async Task SetAccountIdAsync(ViberUser user, Guid accountId)
        {
            await _viberUserRepository.UpdateAccountIdAsync(user, accountId);
        }

        public async Task SetActiveContextAsync(ViberUser user, string? activeContext)
        {
            user.ActiveContext = activeContext;

            await _viberUserRepository.UpdateViberUserAsync(user);
        }

        public async Task SetIsChatEnabledAsync(ViberUser user, bool isEnabled)
        {
            await _viberUserRepository.UpdateChatSettingsAsync(user, isEnabled);

            var internalMessage = new Messaging.Contracts.ViberChatUpdatedEvent
            {
                AccountId = user.AccountId.Value,
                ViberUserId = user.Id,
                IsEnabled = isEnabled,
            };

            await _producer.Publish(internalMessage);
        }

        public async Task UpdateUserAsync(ViberUser user)
        {
            await _viberUserRepository.UpdateViberUserAsync(user);
        }
    }
}
