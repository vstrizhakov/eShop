using AutoMapper;
using eShopping.Catalog.Repositories;
using eShopping.Catalog.Services;
using eShopping.Messaging;
using eShopping.Messaging.Contracts;
using eShopping.Messaging.Contracts.Catalog;

namespace eShopping.Catalog.Tests.Services
{
    public class CurrenciesServiceShould
    {
        [Fact]
        public async Task PublishSyncCurrenciesMessage()
        {
            // Arrange

            var currencies = new[]
            {
                new Entities.Currency
                {
                    Id = Guid.NewGuid(),
                    Name = "USD",
                }
            };

            var currencyRepository = new Mock<ICurrencyRepository>();
            currencyRepository
                .Setup(e => e.GetCurrenciesAsync())
                .ReturnsAsync(currencies);

            var expectedResult = Array.Empty<Currency>();

            var mapper = new Mock<IMapper>();
            mapper
                .Setup(e => e.Map<IEnumerable<Currency>>(currencies))
                .Returns(expectedResult);

            var message = default(SyncCurrenciesMessage);

            var producer = new Mock<IProducer>();
            producer
                .Setup(e => e.Publish(It.IsAny<SyncCurrenciesMessage>()))
                .Callback<SyncCurrenciesMessage>(result => message = result);

            var sut = new SyncService(currencyRepository.Object, mapper.Object, producer.Object);

            // Act

            await sut.SyncAsync();

            // Assert

            currencyRepository.VerifyAll();
            mapper.VerifyAll();
            producer.VerifyAll();

            Assert.NotNull(message);
            Assert.Equal(expectedResult, message.Currencies);
        }
    }
}
