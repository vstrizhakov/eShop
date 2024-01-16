using AutoMapper;
using eShopping.Distribution.Profiles;

namespace eShopping.Distribution.Tests.Profiles
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
