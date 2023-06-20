using eShop.Configurations;
using eShop.Extensions;
using eShop.Models.Clients;
using eShop.Models.TelegramWebhook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Text;

namespace eShop.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        public IActionResult Index([FromServices] IOptions<TelegramBotConfiguration> telegramBotConfiguration)
        {
            var userId = User.GetSub();
            var command = $"{StartContext.Action.RegisterClient},{userId}";
            var context = Convert.ToBase64String(Encoding.UTF8.GetBytes(command));
            var model = new IndexModel
            {
                TelegramInviteLink = $"https://t.me/{telegramBotConfiguration.Value.Username}?start={context}",
            };

            return View(model);
        }
    }
}
