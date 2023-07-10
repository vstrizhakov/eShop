using eShop.Configurations;
using eShop.Database.Data;
using eShop.Models;
using eShop.Models.Clients;
using eShop.Models.TelegramWebhook;
using eShop.MVC.Extensions;
using eShop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace eShop.MVC.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        public async Task<IActionResult> Index(
            [FromServices] IOptions<TelegramBotConfiguration> telegramBotConfiguration,
            [FromServices] IOptions<ViberBotConfiguration> viberBotConfiguration,
            [FromServices] ITelegramContextConverter telegramContextConverter,
            [FromServices] ApplicationDbContext context)
        {
            var providerId = User.GetSub();

            var clients = await context.Users
                .Where(e => e.ProviderId == providerId)
                .ToListAsync();

            var telegramContext = telegramContextConverter.Serialize(TelegramContext.Action.RegisterClient, providerId);
            var viberContext = telegramContextConverter.Serialize(ViberContext.RegisterClient, providerId);

            var model = new IndexModel
            {
                Clients = clients,
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
