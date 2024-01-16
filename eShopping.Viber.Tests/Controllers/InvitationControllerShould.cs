using eShopping.Viber.Controllers;
using eShopping.Viber.Services;

namespace eShopping.Viber.Tests.Controllers
{
    public class InvitationControllerShould
    {
        [Fact]
        public void GetInviteLinkByProviderId()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var inviteLink = Guid.NewGuid().ToString();
            var viberInvitationLinkGenerator = new Mock<IViberInvitationLinkGenerator>();
            viberInvitationLinkGenerator
                .Setup(e => e.Generate(providerId))
                .Returns(inviteLink);

            var sut = new InvitationController();

            // Act

            var result = sut.GetInviteLink(providerId, viberInvitationLinkGenerator.Object);

            // Assert

            viberInvitationLinkGenerator.VerifyAll();

            Assert.NotNull(result);
            Assert.NotNull(result.Value);
            Assert.Equal(inviteLink, result.Value.InviteLink);
        }

        [Fact]
        public void Fail_WhenNotValidProviderId_OnGetInviteLinkByProviderId()
        {
            // Arrange

            var providerId = Guid.NewGuid();
            var inviteLink = Guid.NewGuid().ToString();
            var viberInvitationLinkGenerator = new Mock<IViberInvitationLinkGenerator>();

            var sut = new InvitationController();

            // Act

            var result = sut.GetInviteLink(providerId, viberInvitationLinkGenerator.Object);

            // Assert

            Assert.Fail("Need to implement");
        }
    }
}
