using eShop.Viber.Controllers;
using eShop.Viber.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Viber.Tests.Controllers
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

            var inviteLink = Guid.NewGuid().ToString();
            var viberInvitationLinkGenerator = new Mock<IViberInvitationLinkGenerator>();
            viberInvitationLinkGenerator
                .Setup(e => e.Generate(_accountId))
                .Returns(inviteLink);

            var sut = new InvitationController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = sut.GetInviteLink(viberInvitationLinkGenerator.Object);

            // Assert

            viberInvitationLinkGenerator.VerifyAll();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(inviteLink, result.Value.InviteLink);
        }
    }
}
