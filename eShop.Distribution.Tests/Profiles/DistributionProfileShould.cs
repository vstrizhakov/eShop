using AutoMapper;
using eShop.Distribution.Profiles;

namespace eShop.Distribution.Tests.Profiles
{
    public class DistributionProfileShould
    {
        [Fact]
        public void Map()
        {
            // Arrange

            var configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<DistributionProfile>();
            });

            // Act & Assert

            configuration.AssertConfigurationIsValid();
        }
    }
}
