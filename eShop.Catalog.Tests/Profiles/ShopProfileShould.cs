using AutoMapper;
using eShop.Catalog.Profiles;

namespace eShop.Catalog.Tests.Profiles
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
