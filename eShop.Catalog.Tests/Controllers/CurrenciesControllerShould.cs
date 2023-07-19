using AutoMapper;
using eShop.Catalog.Controllers;
using eShop.Catalog.Entities;
using eShop.Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.Tests.Controllers
{
    public class CurrenciesControllerShould
    {
        [Fact]
        public async Task GetCurrencies()
        {
            // Arrange

            var currencies = GetCurrenciesCollection();

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrenciesAsync())
                .ReturnsAsync(currencies)
                .Verifiable();

            var expectedResponse = Array.Empty<Models.Currencies.Currency>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Models.Currencies.Currency>>(currencies))
                .Returns(expectedResponse)
                .Verifiable();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.GetCurrencies();

            // Assert

            currencyRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.NotNull(result.Result);
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedResponse, (result.Result as OkObjectResult).Value);
        }

        [Fact]
        public async Task ReturnCurrency_OnGetCurrency()
        {
            // Arrange

            var currency = new Currency
            {
                Name = "USD",
            };
            var currencyId = currency.Id;

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrencyByIdAsync(currencyId))
                .ReturnsAsync(currency)
                .Verifiable();

            var expectedResponse = new Models.Currencies.Currency();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Models.Currencies.Currency>(currency))
                .Returns(expectedResponse)
                .Verifiable();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.GetCurrency(currencyId);

            // Assert

            currencyRepository.VerifyAll();
            mapper.VerifyAll();

            Assert.Equal(expectedResponse, result.Value);
        }

        [Fact]
        public async Task ReturnNotFound_OnGetCurrency()
        {
            // Arrange

            var currencyId = Guid.NewGuid();

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrencyByIdAsync(currencyId))
                .ReturnsAsync(default(Currency))
                .Verifiable();

            var expectedResponse = new Models.Currencies.Currency();

            var mapper = new Mock<IMapper>();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.GetCurrency(currencyId);

            // Assert

            currencyRepository.VerifyAll();

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task ReturnCreatedAtAction_OnCreateCurrency()
        {
            // Arrange

            var request = new Models.Currencies.CreateCurrencyRequest
            {
                Name = "USD",
            };

            var currency = new Currency();

            var expectedResponse = new Models.Currencies.Currency();

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.CreateCurrencyAsync(currency))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<Currency>(request))
                .Returns(currency)
                .Verifiable();
            mapper
                .Setup(e => e.Map<Models.Currencies.Currency>(currency))
                .Returns(expectedResponse)
                .Verifiable();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.PostCurrency(request);

            // Assert

            mapper.VerifyAll();
            currencyRepository.VerifyAll();

            Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(expectedResponse, (result.Result as CreatedAtActionResult).Value);
        }

        [Fact]
        public async Task ReturnNoContent_OnDeleteCurrency()
        {
            // Arrange

            var currency = new Currency();
            var currencyId = currency.Id;

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrencyByIdAsync(currencyId))
                .ReturnsAsync(currency)
                .Verifiable();
            currencyRepository
                .Setup(e => e.DeleteCurrencyAsync(currency))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var mapper = new Mock<IMapper>();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.DeleteCurrency(currencyId);

            // Assert

            currencyRepository.VerifyAll();

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task ReturnNotFound_OnDeleteCurrency()
        {
            // Arrange

            var currencyId = Guid.NewGuid();

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrencyByIdAsync(currencyId))
                .ReturnsAsync(default(Currency));

            var mapper = new Mock<IMapper>();

            var controller = new CurrenciesController(currencyRepository.Object, mapper.Object);

            // Act

            var result = await controller.DeleteCurrency(currencyId);

            // Assert

            Assert.IsType<NotFoundResult>(result);
        }

        private IEnumerable<Currency> GetCurrenciesCollection()
        {
            return new[]
            {
                new Currency
                {
                    Name = "USD",
                },
                new Currency
                {
                    Name = "UAH",
                },
            };
        }
    }
}
