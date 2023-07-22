using AutoMapper;
using eShop.Distribution.Controllers;
using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                .Setup(e => e.GetAccountsByProviderIdAsync(_accountId))
                .ReturnsAsync(accounts);

            var expectedResult = Array.Empty<Models.Client>();
            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Models.Client>>(accounts))
                .Returns(expectedResult);

            var controller = new ClientsController
            {
                ControllerContext = _controllerContext,
            };

            // Act

            var result = await controller.GetClients(accountRepository.Object, mapper.Object);

            // Assert

            accountRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResult, (result.Result as OkObjectResult).Value);
        }
    }
}
