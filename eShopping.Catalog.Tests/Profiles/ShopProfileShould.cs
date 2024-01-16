using AutoMapper;
using eShopping.Catalog.Profiles;

namespace eShopping.Catalog.Tests.Profiles
{
    public class ShopProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ShopProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
