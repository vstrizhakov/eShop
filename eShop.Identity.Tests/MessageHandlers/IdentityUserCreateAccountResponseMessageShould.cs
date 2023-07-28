using eShop.Identity.Entities;
using eShop.Identity.MessageHandlers;
using eShop.Messaging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Identity.Tests.MessageHandlers
{
    public class IdentityUserCreateAccountResponseMessageShould
    {
        [Fact]
        public async Task HandleMessage()
        {
            // Arrange 

            var message = new IdentityUserCreateAccountResponseMessage
            {
                IdentityUserId = Guid.NewGuid().ToString(),
                AccountId = Guid.NewGuid(),
            };

            var user = new User();

            var userStore = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            userManager
                .Setup(e => e.FindByIdAsync(message.IdentityUserId))
                .ReturnsAsync(user);
            userManager
                .Setup(e => e.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            var sut = new IdentityUserCreateAccountResponseMessageHandler(userManager.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            userManager.VerifyAll();

            Assert.Equal(message.AccountId, user.AccountId);
        }
    }
}
