using AutoMapper;
using eShop.Distribution.Controllers;
using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.Distribution.Tests.Controllers
{
    public class DistributionControllerShould
    {
        private readonly Guid _accountId = Guid.NewGuid();
        private readonly ControllerContext _controllerContext;

        public DistributionControllerShould()
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
        public async Task ReturnDistribution_OnGetDistribution()
        {
            // Arrange

            var distribution = new Entities.Distribution
            {
                AnnouncerId = _accountId,
            };
            var distributionId = distribution.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionAsync(distributionId))
                .ReturnsAsync(distribution);

            var expectedResult = new Models.Distribution();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Models.Distributions.Distribution>(distribution))
                .Returns(expectedResult);

            var controller = new DistributionsController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetDistribution(distributionId, distributionRepository.Object, mapper.Object);

            // Assert

            distributionRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnNotFound_WhenWrongId_OnGetDistribution()
        {
            // Arrange

            var distributionId = Guid.NewGuid();

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionAsync(distributionId))
                .ReturnsAsync(default(Distribution));

            var expectedResult = new Models.Distribution();

            var mapper = new Mock<IMapper>();

            var controller = new DistributionsController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetDistribution(distributionId, distributionRepository.Object, mapper.Object);

            // Assert

            distributionRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ReturnNotFound_WhenNotOwner_OnGetDistribution()
        {
            // Arrange

            var distribution = new Entities.Distribution();
            var distributionId = distribution.Id;

            var distributionRepository = new Mock<IDistributionRepository>();
            distributionRepository
                .Setup(e => e.GetDistributionAsync(distributionId))
                .ReturnsAsync(distribution);

            var expectedResult = new Models.Distribution();

            var mapper = new Mock<IMapper>();

            var controller = new DistributionsController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetDistribution(distributionId, distributionRepository.Object, mapper.Object);

            // Assert

            distributionRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
