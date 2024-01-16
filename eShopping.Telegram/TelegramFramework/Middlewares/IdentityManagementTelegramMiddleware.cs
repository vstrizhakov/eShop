using eShopping.Telegram.Entities;
using eShopping.TelegramFramework;
using eShopping.Telegram.Entities;
using eShopping.Telegram.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace eShopping.Telegram.TelegramFramework.Middlewares
{
    public class IdentityManagementTelegramMiddleware : ITelegramMiddleware
    {
        private readonly ITelegramUserRepository _telegramUserRepository;
        private readonly ITelegramChatRepository _telegramChatRepository;
        private readonly ITelegramBotClient _botClient;

        public IdentityManagementTelegramMiddleware(
            ITelegramUserRepository telegramUserRepository,
            ITelegramChatRepository telegramChatRepository,
            ITelegramBotClient botClient)
        {
            _telegramUserRepository = telegramUserRepository;
            _telegramChatRepository = telegramChatRepository;
            _botClient = botClient;
        }

        public async Task HandleUpdateAsync(Update update)
        {
            Message? message = null;
            if (update.Type == UpdateType.Message)
            {
                message = update.Message;
            }
            else if (update.Type == UpdateType.ChannelPost)
            {
                message = update.ChannelPost;
            }

            if (message != null)
            {
                var chat = message.Chat;

                var chatId = chat.Id;
                var telegramChat = await _telegramChatRepository.GetTelegramChatByExternalIdAsync(chatId);
                if (telegramChat == null)
                {
                    telegramChat = new TelegramChat
                    {
                        Type = chat.Type,
                        Title = chat.Title,
                        ExternalId = chatId,
                    };

                    await _telegramChatRepository.CreateTelegramChatAsync(telegramChat);

                    if (chat.Type == ChatType.Group || chat.Type == ChatType.Supergroup || chat.Type == ChatType.Channel)
                    {
                        var chatAdministrators = await _botClient.GetChatAdministratorsAsync(new ChatId(chatId));

                        foreach (var chatAdministrator in chatAdministrators)
                        {
                            await AddOrUpdateTelegramUserAsync(telegramChat, chatAdministrator.User, chatAdministrator.Status);
                        }

                        await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                    }
                }
                else
                {
                    telegramChat.Type = chat.Type;
                    telegramChat.Title = chat.Title;

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                }

                if (message.Type == MessageType.MigratedFromGroup)
                {
                    var baseTelegramChat = await _telegramChatRepository.GetTelegramChatByExternalIdAsync(message.MigrateFromChatId!.Value);
                    if (baseTelegramChat != null)
                    {
                        baseTelegramChat.SupergroupId = telegramChat.Id;
                        foreach (var member in baseTelegramChat.Members)
                        {
                            telegramChat.Members.Add(member);
                        }

                        await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                    }
                }

                var from = message.From;
                if (from != null)
                {
                    await AddOrUpdateTelegramUserAsync(telegramChat, from);

                    await _telegramChatRepository.UpdateTelegramChatAsync(telegramChat);
                }
            }
        }

        private async Task AddOrUpdateTelegramUserAsync(TelegramChat telegramChat, User user, ChatMemberStatus? chatMemberStatus = null)
        {
            var userId = user.Id;

            if (chatMemberStatus == null)
            {
                var chatMember = await _botClient.GetChatMemberAsync(telegramChat.ExternalId, userId);
                chatMemberStatus = chatMember.Status;
            }

            var telegramUser = await _telegramUserRepository.GetTelegramUserByExternalIdAsync(userId);
            if (telegramUser == null)
            {
                telegramUser = new TelegramUser
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    ExternalId = userId,
                    Chats =
                    {
                        new EmbeddedTelegramChat
                        {
                            ChatId = telegramChat.Id,
                            Type = telegramChat.Type,
                            Status = chatMemberStatus.Value,
                        },
                    },
                };

                await _telegramUserRepository.CreateTelegramUserAsync(telegramUser);
            }
            else
            {
                telegramUser.FirstName = user.FirstName;
                telegramUser.LastName = user.LastName;
                telegramUser.Username = user.Username;

                var telegramUserChat = telegramUser.Chats.FirstOrDefault(e => e.ChatId == telegramChat.Id);
                if (telegramUserChat == null)
                {
                    telegramUserChat = new EmbeddedTelegramChat
                    {
                        ChatId = telegramChat.Id,
                        Type = telegramChat.Type,
                        Status = chatMemberStatus.Value,
                    };

                    telegramUser.Chats.Add(telegramUserChat);
                }

                await _telegramUserRepository.UpdateTelegramUserAsync(telegramUser);
            }

            {
                var telegramChatMember = telegramChat.Members.FirstOrDefault(e => e.UserId == telegramUser.Id);
                if (telegramChatMember == null)
                {
                    telegramChatMember = new EmbeddedTelegramUser
                    {
                        UserId = telegramUser.Id,
                        Status = chatMemberStatus.Value,
                    };

                    telegramChat.Members.Add(telegramChatMember);
                }
            }
        }
    }
}
