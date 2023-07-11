using eShop.Messaging;
using eShop.Messaging.Models;
using eShop.RabbitMq;
using eShop.Messaging.Extensions;
using eShop.Distribution.Repositories;
using eShop.Distribution.Entities;

namespace eShop.Distribution.MessageHandlers
{
    public class BroadcastCompositionMessageHandler : IMessageHandler<BroadcastCompositionMessage>
    {
        private readonly IRabbitMqProducer _producer;
        private readonly IAccountRepository _accountRepository;
        private readonly IDistributionRepository _distributionRepository;

        public BroadcastCompositionMessageHandler(
            IRabbitMqProducer producer,
            IAccountRepository accountRepository,
            IDistributionRepository distributionRepository)
        {
            _producer = producer;
            _accountRepository = accountRepository;
            _distributionRepository = distributionRepository;
        }

        public async Task HandleMessageAsync(BroadcastCompositionMessage message)
        {
            var composition = message.Composition;
            var image = composition.Images.FirstOrDefault();
            var caption = string.Join("\n\n", composition.Products.Select(e => $"{e.Name} - {e.Price}"));

            var distributionGroup = new DistributionGroup
            {
                ProviderId = message.ProviderId,
            };

            var accounts = await _accountRepository.GetAccountsByProviderIdAsync(message.ProviderId);

            var telegramChatIds = accounts
                .SelectMany(e => e.TelegramChats)
                .Where(e => e.IsEnabled)
                .Select(e => e.Id)
                .Distinct();
            foreach (var telegramChatId in telegramChatIds)
            {
                var distributionGroupItem = new DistributionGroupItem
                {
                    TelegramChatId = telegramChatId,
                };

                distributionGroup.Items.Add(distributionGroupItem);
            }

            var viberChatIds = accounts
                .Where(e => e.ViberChat != null)
                .Select(e => e.ViberChat)
                .Where(e => e.IsEnabled)
                .Select(e => e.Id)
                .Distinct();
            foreach (var viberChatId in viberChatIds)
            {
                var distributionGroupItem = new DistributionGroupItem
                {
                    ViberChatId = viberChatId,
                };

                distributionGroup.Items.Add(distributionGroupItem);
            }

            await _distributionRepository.CreateDistributionGroupAsync(distributionGroup);

            var distributionGroupId = distributionGroup.Id;
            var update = new BroadcastCompositionUpdateEvent
            {
                CompositionId = message.Composition.Id,
                DistributionGroupId = distributionGroupId,
            };

            _producer.Publish(update);

            if (telegramChatIds.Any())
            {
                var telegramMessage = new BroadcastCompositionToTelegramMessage
                {
                    DistributionGroupId = distributionGroupId,
                    TelegramChatIds = telegramChatIds,
                    Image = image,
                    Caption = caption,
                };
                _producer.Publish(telegramMessage);
            }

            if (viberChatIds.Any())
            {
                var viberMessage = new BroadcastCompositionToViberMessage
                {
                    DistributionGroupId = distributionGroupId,
                    ViberChatIds = viberChatIds,
                    Image = image,
                    Caption = caption,
                };
                _producer.Publish(viberMessage);
            }
        }
    }
}
