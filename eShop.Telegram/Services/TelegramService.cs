﻿using eShop.Telegram.Entities;
using eShop.Telegram.Repositories;
using Telegram.Bot.Types.Enums;

namespace eShop.Telegram.Services
{
    public class TelegramService : ITelegramService
    {
        private readonly ITelegramUserRepository _telegramUserRepository;

        public TelegramService(ITelegramUserRepository tegramUserRepository)
        {
            _telegramUserRepository = tegramUserRepository;
        }

        public IEnumerable<TelegramChatMember> GetManagableChats(TelegramUser user)
        {
            var chats = user.Chats
                .Where(e => e.Chat.Type == ChatType.Group || e.Chat.Type == ChatType.Channel || e.Chat.Type == ChatType.Supergroup)
                .Where(e => e.Chat.SupergroupId == null)
                .Where(e => e.Status == ChatMemberStatus.Creator || e.Status == ChatMemberStatus.Administrator);
            return chats;
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
