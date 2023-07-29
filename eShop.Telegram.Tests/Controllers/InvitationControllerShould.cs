using eShop.Telegram.Controllers;
using eShop.Telegram.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Telegram.Tests.Controllers
{
    public class InvitationControllerShould
    {
        private readonly Guid _accountId;
        private readonly ControllerContext _controllerContext;

        public InvitationControllerShould()
        {
            _accountId = Guid.NewGuid();
            var user = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new[]
                {
                    new Claim("account_id", _accountId.ToString()),
                }),
            });
            _controllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user,
                },
            };
        }

        [Fact]
        public void GetInviteLink()
        {
            // Arrange

            var link = "https://t.me/example?start=example";

            var telegramInvitationLinkGenerator = new Mock<ITelegramInvitationLinkGenerator>();
            telegramInvitationLinkGenerator
                .Setup(e => e.Generate(_accountId))
                .Returns(link);

            var sut = new InvitationController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = sut.GetInviteLink(telegramInvitationLinkGenerator.Object);

            // Assert

            telegramInvitationLinkGenerator.VerifyAll();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(link, result.Value.InviteLink);
        }
    }
}
