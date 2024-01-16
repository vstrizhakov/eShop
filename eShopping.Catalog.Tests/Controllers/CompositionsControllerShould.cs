using AutoMapper;
using eShopping.Catalog.Controllers;
using eShopping.Catalog.Models.Announces;
using eShopping.Catalog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShopping.Catalog.Tests.Controllers
{
    public class CompositionsControllerShould
    {
        private readonly Guid _accountId = Guid.NewGuid();
        private readonly ControllerContext _controllerContext;

        public CompositionsControllerShould()
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
        public async Task GetCompositions()
        {
            // Arrange

            var compositions = Array.Empty<Entities.Announce>();

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnouncesAsync(_accountId))
                .ReturnsAsync(compositions);

            var expectedResponse = Array.Empty<Announce>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Announce>>(compositions))
                .Returns(expectedResponse);

            var sut = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.GetCompositions();

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnComposition_OnGetComposition()
        {
            // Arrange

            var composition = new Entities.Announce
            {
                OwnerId = _accountId,
            };
            var compositionId = composition.Id;

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnounceAsync(compositionId))
                .ReturnsAsync(composition);

            var expectedResponse = new Models.Compositions.Announce();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Announce>(composition))
                .Returns(expectedResponse);

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetComposition(compositionId);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task ReturnNotFound_WhenWrongId_OnGetComposition()
        {
            // Arrange

            var composition = new Entities.Announce();
            var compositionId = composition.Id;

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnounceAsync(compositionId))
                .ReturnsAsync(composition);

            var mapper = new Mock<IMapper>();

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetComposition(compositionId);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ReturnNotFound_WhenNotOwner_OnGetComposition()
        {
            // Arrange

            var compositionId = Guid.NewGuid();

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnounceAsync(compositionId))
                .ReturnsAsync(default(Entities.Announce));

            var mapper = new Mock<IMapper>();

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetComposition(compositionId);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ReturnCreatedAtAction_OnCreateComposition()
        {
            // Arrange

            var request = new Models.Compositions.CreateAnnounceRequest();
            var composition = new Entities.Announce();

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.CreateAnnounceAsync(composition, request.Image))
                .Returns(Task.CompletedTask);

            var expectedResponse = new Models.Compositions.Announce();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Entities.Announce>(request))
                .Returns(composition);
            mapper
                .Setup(e => e.Map<Announce>(composition))
                .Returns(expectedResponse);

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.CreateComposition(request);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(expectedResponse, (result.Result as CreatedAtActionResult).Value);
            Assert.Equal(composition.OwnerId, _accountId);
        }

        [Fact]
        public async Task ReturnNoContent_OnDeleteComposition()
        {
            // Arrange

            var composition = new Entities.Announce
            {
                OwnerId = _accountId,
            };
            var compositionId = composition.Id;

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnounceAsync(compositionId))
                .ReturnsAsync(composition);
            compositionService
                .Setup(e => e.DeleteAnnounceAsync(composition))
                .Returns(Task.CompletedTask);

            var mapper = new Mock<IMapper>();

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.DeleteComposition(compositionId);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ReturnNotFound_WhenNotOwner_OnDeleteComposition()
        {
            // Arrange

            var composition = new Entities.Announce();
            var compositionId = composition.Id;

            var compositionService = new Mock<IAnnouncesService>();
            compositionService
                .Setup(e => e.GetAnnounceAsync(compositionId))
                .ReturnsAsync(composition);

            var mapper = new Mock<IMapper>();

            var controller = new AnnouncesController(compositionService.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.DeleteComposition(compositionId);

            // Assert

            compositionService.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
