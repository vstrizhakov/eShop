using eShop.Telegram.Entities;
using eShop.Telegram.Repositories;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly ITelegramChatRepository _telegramChatRepository;

        public TelegramService(ITelegramUserRepository tegramUserRepository, ITelegramChatRepository telegramChatRepository)
        {
            _telegramUserRepository = tegramUserRepository;
            _telegramChatRepository = telegramChatRepository;
        }

        public async Task<IEnumerable<TelegramChat>> GetManagableChats(TelegramUser user)
        {
            var chatIds = user.Chats
                .Where(e => e.Status == ChatMemberStatus.Creator)
                .Where(e => e.Type == ChatType.Group || e.Type == ChatType.Channel || e.Type == ChatType.Supergroup)
                .Select(e => e.ChatId);
            var chats = await _telegramChatRepository.GetTelegramChatsByIdsAsync(chatIds);
            var result = chats
                .Where(e => e.SupergroupId == null);
            return result;
        }

        public async Task<TelegramUser?> GetUserByAccountIdAsync(Guid accountId)
        {
            var user = await _telegramUserRepository.GetTelegramUserByAccountIdAsync(accountId);
            return user;
        }

        public async Task<TelegramUser?> GetUserByExternalIdAsync(long externalId)
        {
            var user = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(externalId);
            return user;
        }

        public async Task<TelegramUser?> GetUserByTelegramUserIdAsync(Guid telegramUserId)
        {
            var user = await _telegramUserRepository.GetTelegramUserByIdAsync(telegramUserId);
            return user;
        }

        public async Task SetAccountIdAsync(TelegramUser user, Guid accountId)
        {
            await _telegramUserRepository.UpdateAccountIdAsync(user, accountId);
        }

        public async Task SetActiveContextAsync(TelegramUser user, string? context)
        {
            user.ActiveContext = context;

            await _telegramUserRepository.UpdateTelegramUserAsync(user);
        }

        public async Task UpdateUserAsync(TelegramUser user)
        {
            await _telegramUserRepository.UpdateTelegramUserAsync(user);
        }
    }
}
