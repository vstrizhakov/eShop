using eShop.Bots.Common;
using eShop.Viber.Services;
using Microsoft.Extensions.Options;

namespace eShop.Viber.Tests.Services
{
    public class ViberInvitationLinkGeneratorShould
    {
        [Fact]
        public void Generate()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var chatUrl = "example";
            var context = "example";

            var botContextConverter = new Mock<IBotContextConverter>();
            botContextConverter
                .Setup(e => e.Serialize(It.IsAny<string[]>()))
                .Returns(context);

            var options = new Mock<IOptions<ViberBotConfiguration>>();
            options
                .Setup(e => e.Value)
                .Returns(new ViberBotConfiguration
                {
                    ChatUrl = chatUrl,
                });

            var sut = new ViberInvitationLinkGenerator(botContextConverter.Object, options.Object);

            // Act

            var result = sut.Generate(providerId);

            // Assert

            botContextConverter.VerifyAll();
            options.VerifyAll();

            Assert.Equal($"viber://pa?chatURI={chatUrl}&context={context}", result);
        }
    }
}
