using AutoMapper;
using eShopping.Distribution.Profiles;

namespace eShopping.Distribution.Tests.Profiles
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
