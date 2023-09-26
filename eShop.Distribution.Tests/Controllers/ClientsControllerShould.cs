using AutoMapper;
using eShop.Distribution.Controllers;
using eShop.Distribution.Entities;
using eShop.Distribution.Models;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eShop.Distribution.Tests.Controllers
{
    public class ClientsControllerShould
    {
        private readonly Guid _accountId = Guid.NewGuid();
        private readonly ControllerContext _controllerContext;

        public ClientsControllerShould()
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
        public async Task GetClients()
        {
            // Arrange

            var accounts = Array.Empty<Account>();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountsByProviderIdAsync(_accountId, null, false))
                .ReturnsAsync(accounts);

            var expectedResult = Array.Empty<Models.Client>();
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Models.Client>>(accounts))
                .Returns(expectedResult);

            var controller = new ClientsController(accountRepository.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetClients();

            // Assert

            accountRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnOk_WhenClientExists_OnActivateClient()
        {
            // Arrange

            var clientId = Guid.NewGuid();
            var account = new Account();
            var expectedResult = new Client();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(clientId, _accountId))
                .ReturnsAsync(account);
            accountRepository
                .Setup(e => e.UpdateIsActivatedAsync(account, true))
                .Returns(Task.CompletedTask);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Client>(account))
                .Returns(expectedResult);

            var sut = new ClientsController(accountRepository.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.ActivateClient(clientId);

            // Assert

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnNotFound_WhenClientNotExists_OnActivateClient()
        {
            // Arrange

            var clientId = Guid.NewGuid();
            var expectedResult = new Client();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(clientId, _accountId))
                .ReturnsAsync(default(Account));

            var mapper = new Mock<IMapper>();

            var sut = new ClientsController(accountRepository.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.ActivateClient(clientId);

            // Assert

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ReturnOk_WhenClientExists_OnDeactivateClient()
        {
            // Arrange

            var clientId = Guid.NewGuid();
            var account = new Account();
            var expectedResult = new Client();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(clientId, _accountId))
                .ReturnsAsync(account);
            accountRepository
                .Setup(e => e.UpdateIsActivatedAsync(account, false))
                .Returns(Task.CompletedTask);

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Client>(account))
                .Returns(expectedResult);

            var sut = new ClientsController(accountRepository.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.DeactivateClient(clientId);

            // Assert

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnNotFound_WhenClientNotExists_OnDeactivateClient()
        {
            // Arrange

            var clientId = Guid.NewGuid();
            var expectedResult = new Client();

            var accountRepository = new Mock<IAccountRepository>();
            accountRepository
                .Setup(e => e.GetAccountByIdAsync(clientId, _accountId))
                .ReturnsAsync(default(Account));

            var mapper = new Mock<IMapper>();

            var sut = new ClientsController(accountRepository.Object, mapper.Object)
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await sut.DeactivateClient(clientId);

            // Assert

            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
