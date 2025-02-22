﻿using eShopping.Messaging.Contracts.Distribution;
using eShopping.Telegram.TelegramFramework.Views;
using eShopping.TelegramFramework;
using eShopping.TelegramFramework.Attributes;
using eShopping.TelegramFramework.Contexts;
using eShopping.Telegram.Models;
using eShopping.Telegram.Services;
using MassTransit;

namespace eShopping.Telegram.TelegramFramework.Controllers
{
    [TelegramController]
    public class SettingsController
    {
        private readonly ITelegramService _telegramService;
        private readonly IRequestClient<GetDistributionSettingsRequest> _getDistributionSettingsRequestClient;
        private readonly IRequestClient<SetShowSalesRequest> _setShowSalesRequestClient;

        public SettingsController(
            ITelegramService telegramService,
            IRequestClient<GetDistributionSettingsRequest> getDistributionSettingsRequestClient,
            IRequestClient<SetShowSalesRequest> setShowSalesRequestClient)
        {
            _telegramService = telegramService;
            _getDistributionSettingsRequestClient = getDistributionSettingsRequestClient;
            _setShowSalesRequestClient = setShowSalesRequestClient;
        }

        [CallbackQuery(TelegramAction.Settings)]
        public async Task<ITelegramView?> ProcessAsync(CallbackQueryContext context)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new GetDistributionSettingsRequest(user.AccountId.Value);
                var result = await _getDistributionSettingsRequestClient.GetResponse<GetDistributionSettingsResponse>(request);
                var response = result.Message;

                return new SettingsView(context.ChatId, context.MessageId, response.DistributionSettings);
            }

            return null;
        }

        [CallbackQuery(TelegramAction.SetShowSales)]
        public async Task<ITelegramView?> SetShowSales(CallbackQueryContext context, bool showSales)
        {
            var user = await _telegramService.GetUserByExternalIdAsync(context.FromId);
            if (user!.AccountId != null)
            {
                var request = new SetShowSalesRequest(user.AccountId.Value, showSales);
                var result = await _setShowSalesRequestClient.GetResponse<SetShowSalesResponse>(request);
                var response = result.Message;

                return new SettingsView(context.ChatId, context.MessageId, response.DistributionSettings);
            }

            return null;
        }
    }
}
