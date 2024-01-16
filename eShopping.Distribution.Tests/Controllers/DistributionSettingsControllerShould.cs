using AutoMapper;
using eShopping.Distribution.Controllers;
using eShopping.Distribution.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShopping.Distribution.Tests.Controllers
{
    public class DistributionSettingsControllerShould
    {
        private readonly Guid _accountId = Guid.NewGuid();
        private readonly ControllerContext _controllerContext;

        public DistributionSettingsControllerShould()
        {
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
        public async Task GetDistributionSettings()
        {
            // Arrange

            var distributionSettings = new Entities.DistributionSettings();
            var expectedResult = new Models.DistributionSettings();

            var distributionSettingsService = new Mock<IDistributionSettingsService>();
            distributionSettingsService
                .Setup(e => e.GetDistributionSettingsAsync(_accountId))
                .ReturnsAsync(distributionSettings);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Models.DistributionSettings>(distributionSettings))
                .Returns(expectedResult);

            var sut = new DistributionSettingsController(distributionSettingsService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.GetDistributionSettings();

            // Assert

            distributionSettingsService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }
    }
}
