using AutoMapper;
using eShopping.Catalog.Profiles;

namespace eShopping.Catalog.Tests.Profiles
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
