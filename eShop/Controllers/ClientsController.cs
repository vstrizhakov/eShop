using eShop.Configurations;
using eShop.Extensions;
using eShop.Models;
using eShop.Models.Clients;
using eShop.Models.TelegramWebhook;
using eShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace eShop.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        public IActionResult Index(
            [FromServices] IOptions<TelegramBotConfiguration> telegramBotConfiguration,
            [FromServices] IOptions<ViberBotConfiguration> viberBotConfiguration,
            [FromServices] ITelegramContextConverter telegramContextConverter)
        {
            var providerId = User.GetSub();
            var telegramContext = telegramContextConverter.Serialize(TelegramContext.Action.RegisterClient, providerId);
            var viberContext = telegramContextConverter.Serialize(ViberContext.RegisterClient, providerId);
            var model = new IndexModel
            {
                TelegramInviteLink = QueryHelpers.AddQueryString($"https://t.me/{telegramBotConfiguration.Value.Username}", "start", telegramContext),
                ViberInviteLink = QueryHelpers.AddQueryString($"viber://pa", new Dictionary<string, string?>(){
                    { "chatURI", viberBotConfiguration.Value.ChatUrl },
                    { "context", viberContext },
                }),
            };

            return View(model);
        }
    }
}
