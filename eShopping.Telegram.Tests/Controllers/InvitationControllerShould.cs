using eShopping.Telegram.Controllers;
using eShopping.Telegram.Services;

namespace eShopping.Telegram.Tests.Controllers
{
    public class InvitationControllerShould
    {
        [Fact]
        public void GetInviteLink()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var link = "https://t.me/example?start=example";

            var telegramInvitationLinkGenerator = new Mock<ITelegramInvitationLinkGenerator>();
            telegramInvitationLinkGenerator
                .Setup(e => e.Generate(providerId))
                .Returns(link);

            var sut = new InvitationController();

            // Act

            var result = sut.GetInviteLink(providerId, telegramInvitationLinkGenerator.Object);

            // Assert

            telegramInvitationLinkGenerator.VerifyAll();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(link, result.Value.InviteLink);
        }

        [Fact]
        public void Fail_WhenInvalidProvider_OnGetInviteLink()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var link = "https://t.me/example?start=example";

            var telegramInvitationLinkGenerator = new Mock<ITelegramInvitationLinkGenerator>();

            var sut = new InvitationController();

            // Act

            var result = sut.GetInviteLink(providerId, telegramInvitationLinkGenerator.Object);

            // Assert

            telegramInvitationLinkGenerator.VerifyAll();

            Assert.Fail("Need to implement");
        }
    }
}
