using AutoMapper;
using eShopping.Distribution.MessageHandlers;
using eShopping.Distribution.Services;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Catalog;

namespace eShopping.Distribution.Tests.MessageHandlers
{
    public class SyncCurrenciesMessageHandlerShould
    {
        [Fact]
        public async Task SyncCurrencies()
        {
            // Arrange

            var requestedCurrencies = Array.Empty<Currency>();
            var message = new SyncCurrenciesMessage
            {
                Currencies = requestedCurrencies,
            };

            var mappedCurrencies = Array.Empty<Entities.Currency>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Entities.Currency>>(requestedCurrencies))
                .Returns(mappedCurrencies);

            var currencyService = new Mock<ICurrencyService>();
            currencyService
                .Setup(e => e.SyncCurrenciesAsync(mappedCurrencies))
                .Returns(Task.CompletedTask);

            var sut = new SyncCurrenciesMessageHandler(mapper.Object, currencyService.Object);

            // Act

            await sut.HandleMessageAsync(message);

            // Assert

            mapper.VerifyAll();
            currencyService.VerifyAll();
        }
    }
}
