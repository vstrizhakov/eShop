using AutoMapper;
using eShopping.Catalog.Profiles;

namespace eShopping.Catalog.Tests.Profiles
{
    public class ProductProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<ProductProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
