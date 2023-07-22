using AutoMapper;
using eShop.Catalog.Profiles;

namespace eShop.Catalog.Tests.Profiles
{
    public class CurrencyProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CurrencyProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
