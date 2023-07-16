using AutoMapper;
using eShop.Catalog.Profiles;

namespace eShop.Catalog.Tests.Profiles
{
    public class CompositionProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<CompositionProfile>();

                config.AddProfile<ProductProfile>();
            });

            // Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
