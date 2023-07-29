using eShop.Bots.Common;
using eShop.Telegram.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Telegram.Tests.Services
{
    public class TelegramInvitationLinkGeneratorShould
    {
        [Fact]
        public void Generate()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var context = "context";
            var username = "username";

            var botContextConverter = new Mock<IBotContextConverter>();
            botContextConverter
                .Setup(e => e.Serialize(It.IsAny<string[]>()))
                .Returns(context);

            var options = new Mock<IOptions<TelegramBotConfiguration>>();
            options
                .Setup(e => e.Value)
                .Returns(new TelegramBotConfiguration
                {
                    Username = username,
                });

            var sut = new TelegramInvitationLinkGenerator(botContextConverter.Object, options.Object);

            // Act

            var result = sut.Generate(providerId);

            // Assert

            Assert.Equal($"https://t.me/{username}?start={context}", result);
        }
    }
}
