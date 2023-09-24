using eShop.Distribution.Entities;
using eShop.Distribution.Repositories;
using eShop.Distribution.Services;

namespace eShop.Distribution.Tests.Services
{
    public class CurrencyServiceShould
    {
        [Fact]
        public async Task SyncCurrencies()
        {
            // Arrange

            var usdCurrency = new Currency
            {
                Id = Guid.NewGuid(),
                Name = "USD",
            };

            var uahCurrency = new Currency
            {
                Id = Guid.NewGuid(),
                Name = "UAH",
            };

            var eurCurrency = new Currency
            {
                Id = Guid.NewGuid(),
                Name = "EUR",
            };

            var existingCurrencies = new[]
            {
                usdCurrency,
                uahCurrency,
            };

            var requestedCurrencies = new[]
            {
                usdCurrency,
                uahCurrency,
                eurCurrency,
            };

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrenciesAsync(It.Is<IEnumerable<Guid>>(o => requestedCurrencies.Select(e => e.Id).SequenceEqual(o))))
                .ReturnsAsync(existingCurrencies);

            currencyRepository
                .Setup(e => e.CreateCurrencyAsync(eurCurrency))
                .Returns(Task.CompletedTask);

            var sut = new CurrencyService(currencyRepository.Object);

            // Act

            await sut.SyncCurrenciesAsync(requestedCurrencies);

            // Assert

            currencyRepository.VerifyAll();
        }
    }
}
